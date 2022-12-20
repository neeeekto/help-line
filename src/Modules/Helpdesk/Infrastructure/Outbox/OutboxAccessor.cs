using System;
using System.Text.Json;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.Outbox;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Outbox
{
    internal class OutboxAccessor : OutboxAccessorBase
    {
        protected override Type GetType(string typeName) => Assemblies.Application.GetType(typeName);

        public OutboxAccessor(IMongoContext context, IMediator mediator) : base(context, mediator)
        {
        }
    }
}
