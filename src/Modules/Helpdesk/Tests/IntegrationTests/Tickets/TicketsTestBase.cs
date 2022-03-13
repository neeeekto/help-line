using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetTicketDelayConfiguration;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using HelpLine.Modules.UserAccess.IntegrationEvents;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets
{
    public abstract class TicketsTestBase : HelpdeskTestBase
    {
        public static string ProjectId = "test";

        protected async Task<string> CreateTicket(TicketTestData testData, bool needCreateProject = true)
        {
            if (needCreateProject)
                await CreateProject();

            var cmd = new CreateTicketCommand(testData.ProjectId, testData.Language, testData.Initiator,
                testData.Tags, testData.Channels, testData.UserMeta,
                null, testData.Message, testData.Source, null);
            return await Module.ExecuteCommandAsync(cmd);
        }

        protected async Task CreateOperator(Guid operatorId)
        {
            var evt = new NewUserCreatedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, operatorId, "test", "test",
                "test");
            BusServiceFactory.PublishInEventBus(evt);
        }

        protected async Task SetDelayConfigForTests()
        {
            var newLifecycleDelay = new Dictionary<TicketLifeCycleType, TimeSpan>
            {
                {TicketLifeCycleType.Resolving, new TimeSpan(1)},
                {TicketLifeCycleType.Feedback, new TimeSpan(1)},
                {TicketLifeCycleType.Closing, new TimeSpan(1)},
            };
            var cmd = new SetTicketDelayConfigurationCommand(ProjectId,
                new ReadOnlyDictionary<TicketLifeCycleType, TimeSpan>(newLifecycleDelay),
                new TimeSpan(1),
                new TimeSpan(1)
            );
            await Module.ExecuteCommandAsync(cmd);
        }

        protected async Task<object> ExecuteAction(string ticketId, TicketActionBase action, InitiatorDto initiator)
        {
            var result = await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, action, initiator));
            return result;
        }

        protected async Task<Guid> Reply(string ticketId, MessageDto message, InitiatorDto initiator)
        {
            var action = new AddOutgoingMessageAction(message);
            var messageId = await ExecuteAction(ticketId, action, initiator);
            return ((TicketOutgoingMessageId) messageId).Value;
        }
    }
}
