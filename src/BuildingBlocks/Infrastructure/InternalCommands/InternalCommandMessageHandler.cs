using System;
using System.Threading.Tasks;
using Autofac;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Infrastructure.Serialization;
using MediatR;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Infrastructure.InternalCommands
{
    public abstract class InternalCommandTaskHandlerBase<T> : IQueueHandler<T> where T: InternalCommandTaskBase
    {
        public async Task Handle(T command)
        {
            using var scope = GetScope();
            var mediator = scope.Resolve<IMediator>();
            Type type = GetType(command.Type);
            var request = JsonConvert.DeserializeObject(command.Data, type, new JsonSerializerSettings
            {
                ContractResolver = new AllPropertiesContractResolver(),
                TypeNameHandling = TypeNameHandling.All
            }) as IRequest;
            if(request == null) return;

            try
            {

                await mediator.Send(request);
            }
            catch (Exception e)
            {
                var repository = scope.Resolve<IInternalCommandTaskRepository>();
                command.Error = e.ToString();
                command.ProcessedDate = DateTime.UtcNow;;
                await repository.InsertAsync(command);
            }
        }

        protected abstract ILifetimeScope GetScope();
        protected abstract Type GetType(string name);

        public Task TryHandle<T1>(T1 msg)
        {
            if (msg is T queueMessage)
                return Handle(queueMessage);
            return Task.CompletedTask;
        }
    }
}
