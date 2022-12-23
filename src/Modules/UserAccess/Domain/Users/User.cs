using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users.Events;
using HelpLine.Modules.UserAccess.Domain.Users.Rules;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public class User : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; }
        public string Email { get; private set; }
        public UserInfo Info { get; private set; }
        public UserStatus Status { get; private set; }
        public UserSecurity Security { get; private set; }
        public UserRoles Roles { get; private set; }
        public IEnumerable<ProjectId> Projects { get; private set; }
        public UserSetup Setup { get; private set; }
        public IEnumerable<PermissionKey> Permissions { get; private set; } // Custom permissions. Used for Admin roles and other services of HL

        public static async Task<User> Create(IUsersChecker usersChecker, string email, UserInfo info, IEnumerable<RoleId> globalRoles,
            IDictionary<ProjectId, IEnumerable<RoleId>> byProjectRoles, IEnumerable<PermissionKey> permissions, IEnumerable<ProjectId> projects)
        {
            var user = new User(email, info, globalRoles, byProjectRoles, permissions, projects);
            await user.CheckRule(new UserEmailMustBeUniqueRule(usersChecker, email));
            return user;
        }

        private User(string email, UserInfo info,
            IEnumerable<RoleId> globalRoles, IDictionary<ProjectId, IEnumerable<RoleId>> byScopeRoles,
            IEnumerable<PermissionKey> permissions, IEnumerable<ProjectId> projects)
        {
            Id = new UserId(Guid.NewGuid());
            Email = email;
            Info = info;
            Status = UserStatus.Active;
            Security = new UserSecurity(Id);
            Roles = new UserRoles(globalRoles, byScopeRoles, Id);
            Permissions = permissions;
            Projects = projects;
            Setup = new UserSetup();
            AddDomainEvent(new UserCreatedDomainEvent(Id, Info.FirstName, Info.LastName, Email));
        }

        public void Delete()
        {
            Status = UserStatus.Deleted;
            Security.Block();
            AddDomainEvent(new UserDeletedDomainEvent(Id));
        }

        public void ChangePermissions(IEnumerable<PermissionKey> permissions)
        {
            Permissions = permissions;
            AddDomainEvent(new UserPermissionsChangedDomainEvent(Id));
        }

        public void ChangeInfo(UserInfo info)
        {
            Info = info;
        }

        public void SetProjects(IEnumerable<ProjectId> projects)
        {
            Projects = projects;
        }

        public void SetSetup(UserSetup setup)
        {
            Setup = setup;
        }
    }
}
