using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.RemoveScenario
{
    public class RemoveScenarioCommand : CommandBase
    {
        public Guid ScenarioId { get; }

        public RemoveScenarioCommand(Guid scenarioId)
        {
            ScenarioId = scenarioId;
        }


    }
}
