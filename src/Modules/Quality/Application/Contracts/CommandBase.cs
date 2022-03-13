using System;

namespace HelpLine.Modules.Quality.Application.Contracts
{
    public abstract class CommandBase : ICommand
    {
        public Guid Id { get; }

        protected CommandBase()
        {
            Id = Guid.NewGuid();
        }

        protected CommandBase(Guid id)
        {
            Id = id;
        }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid Id { get; }

        protected CommandBase()
        {
            Id = Guid.NewGuid();
        }

        protected CommandBase(Guid id)
        {
            Id = id;
        }
    }
}
