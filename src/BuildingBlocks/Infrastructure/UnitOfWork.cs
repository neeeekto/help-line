using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.BuildingBlocks.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly IMongoContext _context;
        private readonly IOutbox _outbox;
        private bool _transaction = false;

        public UnitOfWork(
            IDomainEventsDispatcher domainEventsDispatcher, IMongoContext context, IOutbox outbox)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _context = context;
            _outbox = outbox;
        }

        public bool Transaction => _transaction;
        public event Func<Task> OnCommit;
        public event Func<Task> OnAbort;

        public async Task Begin(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_transaction) return;
            Clear();

            _transaction = true;

            await _context.BeginTransactionAsync(cancellationToken);
        }

        public  async Task Abort(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_transaction)
            {
                _transaction = false;
                await _context.AbortTransactionAsync(cancellationToken);
            }

            if (OnAbort != null)
            {
                await OnAbort.Invoke();
            }
        }

        public async Task Commit(CancellationToken cancellationToken = default(CancellationToken))
        {
            _transaction = false;
            await _domainEventsDispatcher.DispatchEventsAsync();
            await _context.CommitTransactionAsync(cancellationToken: cancellationToken);
            await _outbox.Publish();
            if (OnCommit != null)
            {
                await OnCommit.Invoke();
            }
        }

        private void Clear()
        {
            if (OnCommit != null)
                foreach (Delegate d in OnCommit.GetInvocationList())
                {
                    OnCommit -= (Func<Task>) d;
                }


            if (OnAbort != null)
                foreach (Delegate d in OnAbort.GetInvocationList())
                {
                    OnCommit -= (Func<Task>) d;
                }
        }
    }
}
