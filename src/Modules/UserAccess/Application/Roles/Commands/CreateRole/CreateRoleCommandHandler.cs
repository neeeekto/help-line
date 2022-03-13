using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Domain.Roles;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.CreateRole
{
    class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Guid>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await Role.Create(request.Name, request.Permissions.Select(x => new PermissionKey(x)));
            await _roleRepository.Add(role);
            return role.Id.Value;
        }
    }
}