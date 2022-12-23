using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction
{
    public class ExecuteTicketActionCommand : CommandBase<object>
    {
        public string TicketId { get; }

        public TicketActionBase Action { get; }
        public InitiatorDto Initiator { get; }

        public ExecuteTicketActionCommand(string ticketId, TicketActionBase action, InitiatorDto initiator)
        {
            TicketId = ticketId;
            Action = action;
            Initiator = initiator;
        }
    }
}
