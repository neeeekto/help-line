using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Configuration.Projections;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.PublishTicketViewChangeEvent;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Driver;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Projectors
{
    internal class TicketViewProjector : IProjector
    {
        private readonly IMongoContext _context;
        private readonly ILogger _logger;
        private readonly ICommandsScheduler _scheduler;

        public TicketViewProjector(IMongoContext context, ILogger logger, ICommandsScheduler scheduler)
        {
            _context = context;
            _logger = logger;
            _scheduler = scheduler;
        }

        private IMongoCollection<TicketView> Tickets => _context.GetCollection<TicketView>();
        private IMongoCollection<EventBase<TicketId>> TicketEvents => _context.GetCollection<EventBase<TicketId>>();

        public async Task Project(IEnumerable<IDomainEvent> events)
        {
            var ticketsEvents = events.OfType<TicketEventBase>();
            var eventsByTicket = ticketsEvents.GroupBy(x => x.AggregateId);
            foreach (var ticketEvents in eventsByTicket)
                await Project(ticketEvents.Key, ticketEvents);
        }

        private async Task Project(TicketId ticketId, IEnumerable<TicketEventBase> events)
        {
            var ticket = await GetTicketView(ticketId);
            try
            {
                ticket = Apply(ticket, events);
            }
            catch (Exception e)
            {
                _logger.Error("Error while trying map ticket to ticket view", e);
                ticket = await FullRestoreTicketView(ticketId);
                ticket = Apply(ticket, events);
            }

            await InsertTicketView(ticket);
            await _scheduler.EnqueueAsync(new PublishTicketViewChangeEventCommand(Guid.NewGuid(), ticket.Id, ticket.ProjectId, events.Select(x => x.Id).ToArray()));
        }

        private TicketView Apply(TicketView? ticket, IEnumerable<TicketEventBase> events)
        {
            var builder = new TicketViewBuilder(ticket);
            foreach (var evt in events)
                builder.Project(evt);
            return builder.Ticket;
        }


        private async Task<TicketView?> GetTicketView(TicketId id)
        {
            var ticket = await Tickets.Find(_context.Session, x => x.Id == id.Value).FirstOrDefaultAsync();
            return ticket;
        }

        private async Task InsertTicketView(TicketView ticketView)
        {
            await Tickets.ReplaceOneAsync(_context.Session, x => x.Id == ticketView.Id, ticketView,
                new ReplaceOptions {IsUpsert = true});

        }

        private async Task<TicketView> FullRestoreTicketView(TicketId id)
        {
            var events = await TicketEvents.Find(x => x.AggregateId == id).SortBy(x => x.OccurredOn).ToListAsync();
            if(!events.Any()) throw new ApplicationException($"Events for ticket {id} not found");
            var builder = new TicketViewBuilder();
            foreach (var evt in events)
                builder.Project(evt);
            return builder.Ticket;
        }
    }
}
