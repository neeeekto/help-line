using System.Threading.Tasks;
using Autofac;
using HelpLine.Services.Jobs.Contracts;
using MediatR;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Jobs
{
    internal class JobTaskGenericHandler<T> : JobTaskHandlerBase<T> where T : JobTask
    {
        public override async Task Handle(T task)
        {
            using var scope = UserAccessCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(task);
        }
    }
}
