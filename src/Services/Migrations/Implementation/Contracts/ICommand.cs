using MediatR;

namespace HelpLine.Services.Migrations.Contracts
{
    public interface ICommand<out T> : IRequest<T>
    {

    }

    public interface ICommand : ICommand<Unit> {}
}
