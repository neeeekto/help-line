using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using HelpLine.Modules.UserAccess.IntegrationEvents;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Operators
{
    public abstract class OperatorsTestBase : HelpdeskTestBase
    {
        public Guid OperatorId => UserId;
        protected async Task CreateOperator()
        {
            var evt = new NewUserCreatedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, OperatorId, "test", "test",
                "test");
            BusServiceFactory.PublishInEventBus(evt);
            await BusServiceFactory.EmitAllEvents();
            await BusServiceFactory.EmitAllQueues();
        }
    }
}
