using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.ClearUserSessions;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Roles.Events;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Domain.Users.Events;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Identity
{
    internal class UserSessionCleanerHandler :
        INotificationHandler<UserPermissionsChangedDomainEvent>,
        INotificationHandler<UserRolesChangedDomainEvent>,
        INotificationHandler<RolePermissionsChangedDomainEvent>,
        INotificationHandler<UserBlockedDomainEvent>,
        INotificationHandler<RoleRemovedDomainEvent>
    {
        private readonly ICommandsScheduler _commandsScheduler;
        private readonly IMongoContext _context;

        public UserSessionCleanerHandler(ICommandsScheduler commandsScheduler, IMongoContext context)
        {
            _commandsScheduler = commandsScheduler;
            _context = context;
        }

        public Task Handle(UserPermissionsChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            return ClearSessions(notification.UserId.Value);
        }

        public Task Handle(UserRolesChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            return ClearSessions(notification.UserId.Value);
        }

        public async Task Handle(RolePermissionsChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            var users = await GetUsersByRole(notification.RoleId);
            foreach (var user in users)
                await ClearSessions(user.Id.Value);
        }

        public Task Handle(UserBlockedDomainEvent notification, CancellationToken cancellationToken)
        {
            return ClearSessions(notification.UserId.Value);
        }

        public async Task Handle(RoleRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var users = await GetUsersByRole(notification.RoleId);
            foreach (var user in users)
                await ClearSessions(user.Id.Value);
        }

        private async Task<IEnumerable<User>> GetUsersByRole(RoleId roleId)
        {
            var users = await _context.GetCollection<User>()
                .Find(x => x.Security.IsBlocked == false && x.Status == UserStatus.Active).ToListAsync();

            users = users.Where(x =>
                x.Roles.Global.Contains(roleId) ||
                x.Roles.ByProject.Values.Any(v => v.Contains(roleId))).ToList();
            return users;
        }

        private Task ClearSessions(Guid userId)
        {
            return _commandsScheduler.EnqueueAsync(new ClearUserSessionsCommand(Guid.NewGuid(), userId));
        }
    }
}
