using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers
{
    public class TicketCreatedScenarioTrigger : ScenarioTriggerBase<TicketCreatedEvent>
    {
        private class Checker : TriggerCheckerBase<TicketCreatedScenarioTrigger, TicketCreatedEvent>
        {
            protected override async Task<TriggerCheckResult> Check(TicketCreatedScenarioTrigger trigger,
                TicketCreatedEvent evt, Scenario scenario)
            {
                return TriggerCheckResult.MakeSuccess(new[] {evt.AggregateId});
            }
        }
    }
}
