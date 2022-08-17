using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.SaveAutoreplyScenario
{
    internal class SaveAutoreplyScenarioCommandHandler : ICommandHandler<SaveAutoreplyScenarioCommand, string>
    {
        private readonly IRepository<AutoreplyScenario> _repository;

        public SaveAutoreplyScenarioCommandHandler(IRepository<AutoreplyScenario> repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(SaveAutoreplyScenarioCommand request, CancellationToken cancellationToken)
        {
            await _repository.Update(request.Scenario, true);
            return request.Scenario.Id;
        }
    }
}
