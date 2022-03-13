using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Roles;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.RemoveRole
{
    class RemoveRoleCommandHandler : ICommandHandler<RemoveRoleCommand>
    {
        private readonly IRoleRepository _repository;

        public RemoveRoleCommandHandler(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _repository.Get(new RoleId(request.RoleId));
            if (role != null)
                await role.Remove(_repository);

            return Unit.Value;
        }
    }
}