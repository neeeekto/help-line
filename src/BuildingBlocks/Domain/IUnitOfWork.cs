using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Domain
{
    public interface IUnitOfWork
    {
        bool Transaction { get; }
        event Func<Task> OnCommit;
        event Func<Task> OnAbort;
        Task Begin(CancellationToken cancellationToken = default(CancellationToken));
        Task Abort(CancellationToken cancellationToken = default(CancellationToken));
        Task Commit(CancellationToken cancellationToken = default(CancellationToken));
    }
}
