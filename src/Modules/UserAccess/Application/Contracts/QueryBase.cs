using System;

namespace HelpLine.Modules.UserAccess.Application.Contracts
{
    public abstract class QueryBase
    {
        public Guid Id { get; }

        protected QueryBase()
        {
            Id = Guid.NewGuid();
        }

        protected QueryBase(Guid id)
        {
            Id = id;
        }
    }

    public abstract class QueryBase<TResult> : IQuery<TResult>
    {
        public Guid Id { get; }

        protected QueryBase()
        {
            Id = Guid.NewGuid();
        }

        protected QueryBase(Guid id)
        {
            Id = id;
        }
    }
}
