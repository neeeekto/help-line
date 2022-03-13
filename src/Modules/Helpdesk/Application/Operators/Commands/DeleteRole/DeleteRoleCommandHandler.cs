using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.DeleteRole
{
    internal class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
    {
        private readonly IRepository<OperatorRole> _repository;

        public DeleteRoleCommandHandler(IRepository<OperatorRole> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.RoleId);
            return Unit.Value;
        }
    }
}
