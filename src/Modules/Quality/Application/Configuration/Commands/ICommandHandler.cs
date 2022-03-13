using HelpLine.Modules.Quality.Application.Contracts;
using MediatR;

namespace HelpLine.Modules.Quality.Application.Configuration.Commands
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