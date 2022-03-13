using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users.Events;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public class UserRoles : Entity
    {
        public UserId UserId { get; private set; }

        private List<RoleId> _global;
        public IReadOnlyCollection<RoleId> Global => _global;

        private Dictionary<ProjectId, List<RoleId>> _byProject;

        public IReadOnlyDictionary<ProjectId, IEnumerable<RoleId>> ByProject =>
            new ReadOnlyDictionary<ProjectId, IEnumerable<RoleId>>(_byProject.ToDictionary(x => x.Key,
                x => (IEnumerable<RoleId>) x.Value));

        internal UserRoles(IEnumerable<RoleId> global, IDictionary<ProjectId, IEnumerable<RoleId>> byProject,
            UserId userId)
        {
            UserId = userId;
            _global = global.ToList();
            _byProject = byProject.ToDictionary(x => x.Key, x => x.Value.ToList());
        }

        public void Set(IEnumerable<RoleId> global, IDictionary<ProjectId, IEnumerable<RoleId>> byProject)
        {
            _global = global.ToList();
            _byProject = byProject.ToDictionary(x => x.Key, x => x.Value.ToList());
            AddDomainEvent(new UserRolesChangedDomainEvent(UserId, Global, ByProject));
        }

        public void RemoveAll(IEnumerable<RoleId> roles)
        {
            var needPublishEvent = false;
            foreach (var roleId in roles)
            {
                needPublishEvent |= _global.Remove(roleId);
                foreach (var byProject in _byProject)
                    needPublishEvent |= byProject.Value.Remove(roleId);
            }

            if (needPublishEvent)
                AddDomainEvent(new UserRolesChangedDomainEvent(UserId, Global, ByProject));
        }
    }
}
