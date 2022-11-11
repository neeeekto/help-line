using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.IntegrationEvents;
using HelpLine.Modules.Helpdesk.IntegrationEvents.DTO;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets
{
    internal class TicketEventsToEventBusPublisher :
        INotificationHandler<TicketCreatedEvent>,
        INotificationHandler<TicketClosedEvent>,
        INotificationHandler<TicketStatusChangedEvent>,
        INotificationHandler<TicketOutgoingMessageAddedEvent>,
        INotificationHandler<TicketNotePostedEvent>,
        INotificationHandler<TicketNoteRemovedEvent>,
        INotificationHandler<TicketIncomingMessageAddedEvent>
    {
        private readonly IEventsBus _eventsBus;

        public TicketEventsToEventBusPublisher(IEventsBus eventsBus)
        {
            _eventsBus = eventsBus;
        }


        public Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketCreatedIntegrationEvent(
                notification.Id,
                notification.OccurredOn.Date,
                notification.AggregateId.Value,
                ToInitiator(notification.Initiator),
                notification.ProjectId.Value
            ));
            return Task.CompletedTask;
        }

        public Task Handle(TicketIncomingMessageAddedEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketIncomingMessageAddedIntegrationEvent(
                notification.Id,
                notification.OccurredOn.Date,
                notification.AggregateId.Value,
                ToInitiator(notification.Initiator)
            ));
            return Task.CompletedTask;
        }

        public Task Handle(TicketOutgoingMessageAddedEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketOutgoingMessageAddedIntegrationEvent(
                notification.Id,
                notification.OccurredOn.Date,
                notification.AggregateId.Value,
                ToInitiator(notification.Initiator)
            ));
            return Task.CompletedTask;
        }

        public Task Handle(TicketStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketStatusChangedIntegrationEvent(
                notification.Id,
                notification.OccurredOn.Date,
                notification.AggregateId.Value,
                ToInitiator(notification.Initiator),
                new TicketStatusDto
                {
                    Kind = (Kinds) notification.Status.Kind,
                    Type = (Types) notification.Status.Type,
                }
            ));
            return Task.CompletedTask;
        }

        public Task Handle(TicketClosedEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketClosedIntegrationEvent(
                notification.Id,
                notification.OccurredOn.Date,
                notification.AggregateId.Value,
                ToInitiator(notification.Initiator)
            ));
            return Task.CompletedTask;
        }

        public Task Handle(TicketNotePostedEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketNotePostedIntegrationEvent(
                notification.Id,
                notification.OccurredOn.Date,
                notification.AggregateId.Value,
                ToInitiator(notification.Initiator)
            ));
            return Task.CompletedTask;
        }

        public Task Handle(TicketNoteRemovedEvent notification, CancellationToken cancellationToken)
        {
            _eventsBus.Publish(new TicketNoteRemovedIntegrationEvent(
                notification.Id,
                notification.OccurredOn.Date,
                notification.AggregateId.Value,
                ToInitiator(notification.Initiator)
            ));
            return Task.CompletedTask;
        }

        private InitiatorDto ToInitiator(Initiator initiator)
        {
            switch (initiator)
            {
                case SystemInitiator _:
                    return new SystemInitiatorDto();

                case OperatorInitiator operatorInitiator:
                    return new OperatorInitiatorDto(operatorInitiator.OperatorId.Value);

                case UserInitiator userInitiator:
                    return new UserInitiatorDto(userInitiator.UserId.Value);
                default:
                    throw new ApplicationException($"[{initiator.GetType().FullName}]: Unknown initiator type");
            }
        }
    }
}
