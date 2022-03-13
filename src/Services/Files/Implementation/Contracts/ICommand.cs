using MediatR;

namespace HelpLine.Services.Files.Contracts
{
    public interface ICommand<out T> : IRequest<T>
    {

    }

    public interface ICommand : ICommand<Unit> {}
}
