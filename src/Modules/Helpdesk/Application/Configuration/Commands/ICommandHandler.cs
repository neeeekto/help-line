using HelpLine.Modules.Helpdesk.Application.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Configuration.Commands
{
    public interface ICommandHandler<in TCommand> : 
        IRequestHandler<TCommand> where TCommand : ICommand
    {
        
    }

    public interface ICommandHandler<in TCommand, TResult> : 
        IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {

    }
}
