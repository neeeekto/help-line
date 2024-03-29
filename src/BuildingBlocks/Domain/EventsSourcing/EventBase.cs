﻿using System;

namespace HelpLine.BuildingBlocks.Domain.EventsSourcing
{
    public abstract class EventBase<TId> : DomainEventBase
    {
        // TODO: twitter snowflake id ?
        public Guid Id { get; private set; }
        public TId AggregateId { get; private set; }

        protected EventBase(TId aggregateId)
        {
            Id = Guid.NewGuid();
            AggregateId = aggregateId;
        }
    }
}
