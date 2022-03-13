using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;

namespace HelpLine.BuildingBlocks.Infrastructure.EventSourcing
{
    public interface ISnapshotStore<TId, TState>
        where TState : IEventsSourcingAggregateState
    {
        Task Add(TId aggregateId, int version, TState state);
        Task<AggregateStateSnapshot<TId, TState>> Get(TId aggregateId);
    }
}