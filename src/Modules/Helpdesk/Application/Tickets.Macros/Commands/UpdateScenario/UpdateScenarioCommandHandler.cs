using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.UpdateScenario
{
    class UpdateScenarioCommandHandler : ICommandHandler<UpdateScenarioCommand, Scenario>
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly TriggerInstallerService _triggerInstaller;

        public UpdateScenarioCommandHandler(IScenarioRepository scenarioRepository,
            TriggerInstallerService triggerInstaller)
        {
            _scenarioRepository = scenarioRepository;
            _triggerInstaller = triggerInstaller;
        }

        public async Task<Scenario> Handle(UpdateScenarioCommand request, CancellationToken cancellationToken)
        {
            var scenario = await _scenarioRepository.FindOne(x => x.Id == request.ScenarioId);
            if (scenario == null)
                throw new NotFoundException(request.ScenarioId);

            scenario.Filters = request.Filters;
            scenario.Name = request.Name;
            scenario.Description = request.Description;
            scenario.Weight = request.Weight;
            scenario.Actions = request.Actions;
            scenario.Enabled = false;
            scenario.ErrorBehavior = request.ErrorBehavior;

            await _triggerInstaller.Uninstall(scenario, scenario.Triggers);
            scenario.Triggers = request.Triggers;

            await _scenarioRepository.Update(scenario);

            return scenario;
        }
    }
}
