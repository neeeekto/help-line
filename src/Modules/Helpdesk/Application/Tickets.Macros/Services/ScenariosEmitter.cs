using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services
{
    internal class ScenariosEmitter :
        INotificationHandler<TicketCreatedEvent>,
        INotificationHandler<TicketStatusChangedEvent>

    {
        private readonly IMongoContext _context;
        private readonly ScenariosRunner _runner;
        private readonly ICommandsScheduler _scheduler;

        protected ScenariosEmitter(IMongoContext context, ScenariosRunner runner, ICommandsScheduler scheduler)
        {
            _context = context;
            _runner = runner;
            _scheduler = scheduler;
        }


        protected async Task CheckAndEnqueue(INotification evt)
        {
            var evtName = ScenarioTriggerBase.GetEventName(evt);
            var scenarios =
                await _context
                    .GetCollection<Scenario>()
                    .Find(x =>
                        x.Triggers.Any(x => x.Event == evtName)
                        && x.Enabled
                    )
                    .ToListAsync();
            foreach (var scenario in scenarios)
                _runner.Add(scenario, evt);
        }

        public Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken)
        {
            return CheckAndEnqueue(notification);
        }

        public Task Handle(TicketStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            return CheckAndEnqueue(notification);
        }
    }
}
