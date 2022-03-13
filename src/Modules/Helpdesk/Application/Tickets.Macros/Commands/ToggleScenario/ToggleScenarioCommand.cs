using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.ToggleScenario
{
    public class ToggleScenarioCommand : CommandBase
    {
        public Guid ScenarioId { get; }
        public bool Enabled { get; }

        public ToggleScenarioCommand(Guid scenarioId, bool enabled)
        {
            ScenarioId = scenarioId;
            Enabled = enabled;
        }


    }
}
