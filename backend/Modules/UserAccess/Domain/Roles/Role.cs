using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Domain.Roles.Events;

namespace HelpLine.Modules.UserAccess.Domain.Roles
{
    public class Role : Entity, IAggregateRoot
    {
        public RoleId Id { get; private set; }
        public string Name { get; private set; }

        private List<PermissionKey> _permissions;
        public IReadOnlyCollection<PermissionKey> Permissions => _permissions;

        public static async Task<Role> Create(string name, IEnumerable<PermissionKey> permissions)
        {
            var role = new Role(permissions, name);
            return role;
        }

        private Role(IEnumerable<PermissionKey> permissions, string name)
        {
            Id = new RoleId(Guid.NewGuid());
            _permissions = permissions.ToList();
            Name = name;
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public async Task Remove(IRoleRepository repository)
        {
            AddDomainEvent(new RoleRemovedDomainEvent(Id));
            await repository.Remove(Id);
        }


        public void ChangePermissions(IEnumerable<PermissionKey> permissions)
        {
            _permissions = permissions.ToList();
            AddDomainEvent(new RolePermissionsChangedDomainEvent(Id, Permissions));
        }
    }
}
