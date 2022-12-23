using MediatR;

namespace HelpLine.Services.TemplateRenderer.Contracts
{
    public interface ICommand<out T> : IRequest<T>
    {

    }

    public interface ICommand : ICommand<Unit> {}
}
