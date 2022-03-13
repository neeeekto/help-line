using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.UpdateRole
{
    internal class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
    {
        private readonly IRepository<OperatorRole> _repository;

        public UpdateRoleCommandHandler(IRepository<OperatorRole> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _repository.FindOne(x => x.Id == request.RoleId);
            if (role == null)
            {
                throw new NotFoundException(request.RoleId);
            }
            role.Data = request.Data;
            await _repository.Update(role);
            return Unit.Value;
        }
    }
}
