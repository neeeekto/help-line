using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Domain.Roles;

namespace HelpLine.Modules.UserAccess.Domain.Users.Events
{
    public class UserRolesChangedDomainEvent : DomainEventBase
    {
        public UserId UserId { get; }
        public IReadOnlyCollection<RoleId> Global { get; }
        public IReadOnlyDictionary<ProjectId, IEnumerable<RoleId>> ByProject { get; }

        public UserRolesChangedDomainEvent(UserId userId, IReadOnlyCollection<RoleId> global,
            IReadOnlyDictionary<ProjectId, IEnumerable<RoleId>> byProject)
        {
            UserId = userId;
            Global = global;
            ByProject = byProject;
        }
    }
}
