using MediatR;

namespace HelpLine.Services.Jobs.Contracts
{
    public interface ICommand<out T> : IRequest<T>
    {

    }

    public interface ICommand : ICommand<Unit> {}
}
