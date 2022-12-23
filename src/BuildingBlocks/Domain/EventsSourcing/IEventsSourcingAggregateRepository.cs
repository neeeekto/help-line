using System.Threading;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Domain.EventsSourcing
{
    public interface IEventsSourcingAggregateRepository<in TId, TAggregate>
        where TAggregate : IEventsSourcingAggregate<TId>
    {
        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
        Task<TAggregate> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    }
}
