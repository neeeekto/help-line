using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.RemoveAutoreplyScenario
{
    internal class RemoveAutoreplyScenarioCommandHandler : ICommandHandler<RemoveAutoreplyScenarioCommand>
    {
        private readonly IRepository<AutoreplyScenario> _repository;

        public RemoveAutoreplyScenarioCommandHandler(IRepository<AutoreplyScenario> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveAutoreplyScenarioCommand request, CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.ScenarioId);
            return Unit.Value;
        }
    }
}
