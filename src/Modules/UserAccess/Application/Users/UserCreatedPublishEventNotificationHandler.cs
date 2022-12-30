using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.UserAccess.Application.Users.Notifications;
using HelpLine.Modules.UserAccess.IntegrationEvents;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Users
{
    internal class UserCreatedPublishEventNotificationHandler : INotificationHandler<UserCreatedNotification>
    {
        private readonly IEventsBus _eventsBus;

        public UserCreatedPublishEventNotificationHandler(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }


        public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
        {
            await _eventsBus.Publish(new NewUserCreatedIntegrationEvent(notification.Id, notification.DomainEvent.OccurredOn,
                notification.DomainEvent.UserId.Value,
                notification.DomainEvent.Email,
                notification.DomainEvent.FirstName,
                notification.DomainEvent.LastName));

        }
    }
}
