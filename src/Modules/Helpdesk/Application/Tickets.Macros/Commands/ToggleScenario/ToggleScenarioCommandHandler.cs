using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.ToggleScenario
{
    class ToggleScenarioCommandHandler : ICommandHandler<ToggleScenarioCommand>
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly TriggerInstallerService _triggerInstaller;

        public ToggleScenarioCommandHandler(IScenarioRepository scenarioRepository, TriggerInstallerService triggerInstaller)
        {
            _scenarioRepository = scenarioRepository;
            _triggerInstaller = triggerInstaller;
        }

        public async Task<Unit> Handle(ToggleScenarioCommand request, CancellationToken cancellationToken)
        {
            var scenario = await _scenarioRepository.FindOne(x => x.Id == request.ScenarioId);
            if (scenario == null)
                throw new NotFoundException(request.ScenarioId);

            if (scenario.Enabled != request.Enabled)
            {
                scenario.Enabled = request.Enabled;
                await _scenarioRepository.Update(scenario);
                if (scenario.Enabled)
                    await _triggerInstaller.Install(scenario, scenario.Triggers);
                else
                    await _triggerInstaller.Uninstall(scenario, scenario.Triggers);
            }
            return Unit.Value;
        }
    }
}
