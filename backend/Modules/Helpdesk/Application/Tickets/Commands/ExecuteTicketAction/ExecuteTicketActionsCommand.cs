using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction
{
    public class ExecuteTicketActionsCommand : CommandBase<IEnumerable<object>>
    {
        public string TicketId { get; }

        public IEnumerable<TicketActionBase> Actions { get; }
        public InitiatorDto Initiator { get; }

        public ExecuteTicketActionsCommand(string ticketId, IEnumerable<TicketActionBase> actions,
            InitiatorDto initiator)
        {
            TicketId = ticketId;
            Actions = actions;
            Initiator = initiator;
        }

        public ExecuteTicketActionsCommand(string ticketId, InitiatorDto initiator, params TicketActionBase[] actions)
        {
            TicketId = ticketId;
            Initiator = initiator;
            Actions = actions;
        }
    }
}
