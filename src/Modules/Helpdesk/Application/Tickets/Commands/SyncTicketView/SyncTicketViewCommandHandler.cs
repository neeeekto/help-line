using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Projectors;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SyncTicketView
{
    internal class SyncTicketViewCommandHandler : ICommandHandler<SyncTicketViewCommand>,
        ICommandHandler<SyncTicketsViewsCommand>,
        ICommandHandler<RecreateTicketViewCommand>
    {
        private readonly ITicketViewRepository _ticketViewRepository;
        private readonly IEventStore<TicketId> _eventStore;
        private readonly ICommandsScheduler _commandsScheduler;
        private readonly ITicketSearchProvider _searchProvider;
        private readonly ILogger _logger;


        public SyncTicketViewCommandHandler(ICommandsScheduler commandsScheduler, ITicketSearchProvider searchProvider,
            ILogger logger, ITicketViewRepository ticketViewRepository, IEventStore<TicketId> eventStore)
        {
            _commandsScheduler = commandsScheduler;
            _searchProvider = searchProvider;
            _logger = logger;
            _ticketViewRepository = ticketViewRepository;
            _eventStore = eventStore;
        }

        public async Task<Unit> Handle(SyncTicketViewCommand request, CancellationToken cancellationToken)
        {
            await Sync(request.TicketId, cancellationToken);
            return Unit.Value;
        }

        public async Task<Unit> Handle(SyncTicketsViewsCommand request, CancellationToken cancellationToken)
        {
            var ticketsIds =
                await _searchProvider.FindIds(request.Filter, new[] { new TicketIdSort() { Descending = false } });
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
            var events = await _eventStore.Get(new TicketId(ticketId), 0);
            var builder = new TicketViewBuilder();
            var ticketView = builder.Project(events).Ticket;
            await _ticketViewRepository.Save(ticketView, cancellationToken);
        }
    }
}
