using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;

namespace HelpLine.BuildingBlocks.Infrastructure.EventSourcing
{
    public interface IEventStore<TId>
    {
        Task<IEnumerable<EventBase<TId>>> Get(TId aggregateId, int fromVersion);
        Task<IEnumerable<EventBase<TId>>> Get(TId aggregateId, DateTime atDate);
        Task Add(IEnumerable<EventBase<TId>> events);
    }
}
