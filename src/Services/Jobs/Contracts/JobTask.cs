using System;
using MediatR;

namespace HelpLine.Services.Jobs.Contracts
{

    public abstract class JobTask : IRequest
    {
        public Guid Id { get; }

        protected JobTask(Guid id)
        {
            Id = id;
        }
    }

    public abstract class JobTask<TData> : JobTask where TData : JobDataBase
    {
        public TData Data { get; }

        protected JobTask(Guid id, TData data) : base(id)
        {
            Data = data;
        }
    }


}
