using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.SaveAutoreplyScenario
{
    public class SaveAutoreplyScenarioCommand : CommandBase<string>
    {
        public AutoreplyScenario Scenario { get; }

        public SaveAutoreplyScenarioCommand(AutoreplyScenario scenario)
        {
            Scenario = scenario;
        }
    }
}
