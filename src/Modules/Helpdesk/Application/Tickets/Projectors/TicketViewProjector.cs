using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Configuration.Projections;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.PublishTicketViewChangeEvent;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Projectors
{
    internal class TicketViewProjector : IProjector
    {
        private readonly ILogger _logger;
        private readonly ICommandsScheduler _scheduler;
        private readonly ITicketViewRepository _repository;
        private readonly IEventStore<TicketId> _eventStore;


        public TicketViewProjector(ILogger logger, ICommandsScheduler scheduler, ITicketViewRepository repository, IEventStore<TicketId> eventStore)
        {
            _logger = logger;
            _scheduler = scheduler;
            _repository = repository;
            _eventStore = eventStore;
        }


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
            await _scheduler.EnqueueAsync(new PublishTicketViewChangeEventCommand(Guid.NewGuid(), ticket.Id,
                ticket.ProjectId, events.Select(x => x.Id).ToArray()));
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
            var ticket = await _repository.Get(id.Value);
            return ticket;
        }

        private async Task InsertTicketView(TicketView ticketView)
        {
            await _repository.Save(ticketView);
        }

        private async Task<TicketView> FullRestoreTicketView(TicketId id)
        {
            var events = await _eventStore.Get(id, 0);
            if (!events.Any()) throw new ApplicationException($"Events for ticket {id} not found");
            var builder = new TicketViewBuilder();
            foreach (var evt in events)
                builder.Project(evt);
            return builder.Ticket;
        }
    }
}
