﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILifetimeScope _scope;
        private readonly IOutbox _outbox;
        private readonly IDomainEventsAccessor _domainEventsProvider;
        private readonly IEventsBus _eventsBus;

        public DomainEventsDispatcher(
            IMediator mediator,
            ILifetimeScope scope,
            IOutbox outbox,
            IDomainEventsAccessor domainEventsProvider, IEventsBus eventsBus)
        {
            _mediator = mediator;
            _scope = scope;
            _outbox = outbox;
            _domainEventsProvider = domainEventsProvider;
            _eventsBus = eventsBus;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEvents = _domainEventsProvider.GetAllDomainEvents();


            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();
            foreach (var domainEvent in domainEvents)
            {
                Type domainEvenNotificationType = typeof(IDomainEventNotification<>);
                var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent),
                    new NamedParameter("id", Guid.NewGuid())
                });

                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
                }
            }

            _domainEventsProvider.ClearAllDomainEvents();


            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent);

            foreach (var domainEventNotification in domainEventNotifications)
            {
                string type = domainEventNotification.GetType().FullName;
                var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                });
                OutboxMessage outboxMessage = new OutboxMessage(
                    domainEventNotification.Id,
                    domainEventNotification.DomainEvent.OccurredOn,
                    type,
                    data);
                await _outbox.Add(outboxMessage);
            }
        }
    }
}
