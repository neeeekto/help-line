using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services
{
    internal static class ScenarioInitiatorFactory
    {
        public static InitiatorDto Make(ScenarioInfo scenarioInfo)
        {
            return new SystemInitiatorDto
            {
                Description = $"Scenario:{scenarioInfo.Name}",
                Meta = new Dictionary<string, string>
                {
                    {"id", scenarioInfo.Id.ToString()}
                }
            };
        }
    }
}
