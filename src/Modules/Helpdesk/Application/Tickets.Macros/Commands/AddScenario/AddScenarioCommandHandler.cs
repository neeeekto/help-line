using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Services;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.AddScenario
{
    class AddScenarioCommandHandler : ICommandHandler<AddScenarioCommand, Scenario>
    {
        private readonly IScenarioRepository _scenarioRepository;
        private readonly TriggerInstallerService _triggerInstallerService;

        public AddScenarioCommandHandler(IScenarioRepository scenarioRepository, TriggerInstallerService triggerInstallerService)
        {
            _scenarioRepository = scenarioRepository;
            _triggerInstallerService = triggerInstallerService;
        }

        public async Task<Scenario> Handle(AddScenarioCommand request, CancellationToken cancellationToken)
        {
            var scenario = new Scenario
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                Name = request.Name,
                Actions = request.Actions,
                Triggers = request.Triggers,
                Filters = request.Filters,
                Weight = request.Weight,
                Enabled = false,
                ErrorBehavior = request.ErrorBehavior
            };
            await _scenarioRepository.Add(scenario);
            return scenario;
        }
    }
}
