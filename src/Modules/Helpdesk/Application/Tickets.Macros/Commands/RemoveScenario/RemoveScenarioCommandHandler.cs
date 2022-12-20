using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.RemoveScenario
{
    class RemoveScenarioCommandHandler : ICommandHandler<RemoveScenarioCommand>
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly TriggerInstallerService _triggerInstaller;

        public RemoveScenarioCommandHandler(IScenarioRepository scenarioRepository, TriggerInstallerService triggerInstaller)
        {
            _scenarioRepository = scenarioRepository;
            _triggerInstaller = triggerInstaller;
        }

        public async Task<Unit> Handle(RemoveScenarioCommand request, CancellationToken cancellationToken)
        {
            var scenario = await _scenarioRepository.FindOne(x => x.Id == request.ScenarioId);
            if (scenario == null)
                return Unit.Value;

            await _triggerInstaller.Uninstall(scenario, scenario.Triggers);
            await _scenarioRepository.Remove(x => x.Id == scenario.Id);
            return Unit.Value;
        }
    }
}
