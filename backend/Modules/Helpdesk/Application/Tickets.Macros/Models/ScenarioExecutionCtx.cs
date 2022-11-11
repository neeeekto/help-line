using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models
{
    internal class ScenarioExecutionCtx
    {
        public ScenarioInfo ScenarioInfo { get; }
        public IEnumerable<TicketActionBase> Actions { get; }

        public ScenarioExecutionCtx(ScenarioInfo scenarioInfo, IEnumerable<TicketActionBase> actions)
        {
            ScenarioInfo = scenarioInfo;
            Actions = actions;
        }
    }
}
