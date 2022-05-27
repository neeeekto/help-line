using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SyncTicketView;
using HelpLine.Modules.Helpdesk.Application.Tickets.Projectors;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RecreateTicketView
{
    internal class SyncTicketViewCommandHandler : ICommandHandler<SyncTicketViewCommand>, 
        ICommandHandler<SyncTicketsViewsCommand>,
        ICommandHandler<RecreateTicketViewCommand>
    {
        private readonly IEventStore<TicketId> _events;
        private readonly IMongoContext _context;
        private readonly ICommandsScheduler _commandsScheduler;
        private readonly ITicketSearchProvider _searchProvider;
        private readonly ILogger _logger;


        public SyncTicketViewCommandHandler(IEventStore<TicketId> events, IMongoContext context, ICommandsScheduler commandsScheduler, ITicketSearchProvider searchProvider, ILogger logger)
        {
            _events = events;
            _context = context;
            _commandsScheduler = commandsScheduler;
            _searchProvider = searchProvider;
            _logger = logger;
        }

        public async Task<Unit> Handle(SyncTicketViewCommand request, CancellationToken cancellationToken)
        {
            await Sync(request.TicketId, cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(SyncTicketsViewsCommand request, CancellationToken cancellationToken)
        {
            var ticketsIds = await _searchProvider.FindIds(request.Filter, new [] {new TicketIdSort() {Descending = false}});
            await Task.WhenAll(ticketsIds.Select(async ticketsId =>
            {
                try
                {
                    await _commandsScheduler.EnqueueAsync(new RecreateTicketViewCommand(Guid.NewGuid(), ticketsId), 0);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Can't run view sync for ticket {TicketsId}", ticketsId);
                }
            }));
            return Unit.Value;
        }

        public async Task<Unit> Handle(RecreateTicketViewCommand request, CancellationToken cancellationToken)
        {
            await Sync(request.TicketId, cancellationToken);
            return Unit.Value;
        }

        private async Task Sync(string ticketId, CancellationToken cancellationToken)
        {
            var events = await _events.Get(new TicketId(ticketId), 0);
            var builder = new TicketViewBuilder();
            var ticketView = builder.Project(events).Ticket;
            await _context.GetCollection<TicketView>().ReplaceOneAsync(_context.Session, x => x.Id == ticketView.Id, ticketView,
                new ReplaceOptions {IsUpsert = true}, cancellationToken: cancellationToken);
        }
    }
}
