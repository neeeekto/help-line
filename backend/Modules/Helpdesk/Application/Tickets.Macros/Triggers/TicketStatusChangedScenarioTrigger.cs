using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers
{
    public class TicketStatusChangedScenarioTrigger : ScenarioTriggerBase<TicketStatusChangedEvent>
    {
        public TicketStatus From { get; }

        private class Checker : TriggerCheckerBase<TicketStatusChangedScenarioTrigger, TicketStatusChangedEvent>
        {
            private readonly IMongoContext _context;

            public Checker(IMongoContext context)
            {
                _context = context;
            }

            protected override async Task<TriggerCheckResult> Check(TicketStatusChangedScenarioTrigger trigger,
                TicketStatusChangedEvent evt, Scenario scenario)
            {
                var ticketView = await _context
                    .GetCollection<TicketView>()
                    .Find(x => x.Id == evt.AggregateId.Value)
                    .FirstOrDefaultAsync();

                var viewEvent = ticketView.Events.OfType<TicketStatusChangedEventView>()
                    .FirstOrDefault(x => x.Id == evt.Id);
                var newStatus = new TicketStatusView(trigger.From);

                return viewEvent?.Old?.Equals(newStatus) == true
                    ? TriggerCheckResult.MakeSuccess(new[] {evt.AggregateId})
                    : TriggerCheckResult.MakeFail();
            }
        }
    }
}
