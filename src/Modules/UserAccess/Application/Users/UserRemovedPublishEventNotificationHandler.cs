using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.UserAccess.Application.Users.Notifications;
using HelpLine.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users
{
    internal class UserRemovedPublishEventNotificationHandler : INotificationHandler<UserRemovedNotification>
    {
        private readonly IEventsBus _eventsBus;

        public UserRemovedPublishEventNotificationHandler(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }


        public async Task Handle(UserRemovedNotification notification, CancellationToken cancellationToken)
        {
            await _eventsBus.Publish(new UserRemovedIntegrationEvent(notification.Id, notification.DomainEvent.OccurredOn,
                notification.DomainEvent.UserId.Value));

        }
    }
}
