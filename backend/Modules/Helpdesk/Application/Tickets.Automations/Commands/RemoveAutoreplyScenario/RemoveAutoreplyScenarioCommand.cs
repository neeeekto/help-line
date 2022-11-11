using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.RemoveAutoreplyScenario
{
    public class RemoveAutoreplyScenarioCommand : CommandBase
    {
        public string ScenarioId { get; }

        public RemoveAutoreplyScenarioCommand(string scenarioId)
        {
            ScenarioId = scenarioId;
        }
    }
}
