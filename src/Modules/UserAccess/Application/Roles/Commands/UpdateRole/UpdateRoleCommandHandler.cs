using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Roles;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.UpdateRole
{
    class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.Get(new RoleId(request.RoleId));
            if (role == null) throw new NotFoundException(request.RoleId);

            role.ChangeName(request.Name);
            role.ChangePermissions(request.Permissions.Select(x => new PermissionKey(x)));
            await _roleRepository.Update(role);
            return Unit.Value;
        }
    }
}