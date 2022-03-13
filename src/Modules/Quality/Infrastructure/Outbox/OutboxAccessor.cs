using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.Outbox;
using HelpLine.Modules.Quality.Infrastructure.Configuration;
using MediatR;
using Newtonsoft.Json;

namespace HelpLine.Modules.Quality.Infrastructure.Outbox
{
    internal class OutboxAccessor : OutboxAccessorBase
    {
        protected override Type GetType(string typeName) => Assemblies.Application.GetType(typeName);

        public OutboxAccessor(IMongoContext context, IMediator mediator) : base(context, mediator)
        {
        }
    }
}
