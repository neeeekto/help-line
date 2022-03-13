using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Application.Roles.Notifications;
using HelpLine.Modules.UserAccess.Application.Users.Commands;
using HelpLine.Modules.UserAccess.Application.Users.Commands.RemoveRemovedRoleInUser;
using HelpLine.Modules.UserAccess.Application.Users.Queries;
using HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsersByRole;
using HelpLine.Modules.UserAccess.Domain.Users;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Users
{
    internal class RemoveRoleFromUserHandler : INotificationHandler<RoleRemovedNotificationEvent>
    {
        private readonly ICommandsScheduler _commandsScheduler;
        private readonly IMediator _mediator;

        public RemoveRoleFromUserHandler(ICommandsScheduler commandsScheduler, IMediator mediator)
        {
            _commandsScheduler = commandsScheduler;
            _mediator = mediator;
        }

        public async Task Handle(RoleRemovedNotificationEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Поля не мапятся в монгу, пока хз как выбирать
            var users = await _mediator.Send(new GetUsersByRoleQuery(notification.DomainEvent.RoleId.Value));
            foreach (var user in users)
                await _commandsScheduler.EnqueueAsync(new RemoveRemovedRoleInUserCommand(notification.Id, user.Id.Value,
                    notification.DomainEvent.RoleId.Value));
        }
    }
}
