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
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using HelpLine.Modules.UserAccess.IntegrationEvents;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    public abstract class TicketsTestBase : HelpdeskTestBase
    {
        public static string ProjectId = HelpdeskTestBase.ProjectId;

        public async Task<Guid> CreateOperator(Guid operatorId, string email = "test", string firstName = "test", string lastName = "test")
        {
            var evt = new NewUserCreatedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, operatorId, email, firstName,
                lastName);
            BusServiceFactory.PublishInEventBus(evt);
            return operatorId; 
        }

        public async Task SetDelayConfigForTests()
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

        public async Task<object> ExecuteAction(string ticketId, TicketActionBase action, InitiatorDto initiator)
        {
            var result = await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, action, initiator));
            return result;
        }

        public async Task<Guid> Reply(string ticketId, MessageDto message, InitiatorDto initiator)
        {
            var action = new AddOutgoingMessageAction(message);
            var messageId = await ExecuteAction(ticketId, action, initiator);
            return ((TicketOutgoingMessageId) messageId).Value;
        }
    }
}
