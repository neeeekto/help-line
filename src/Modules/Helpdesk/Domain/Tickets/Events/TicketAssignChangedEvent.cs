#nullable enable
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketAssignChangedEvent : TicketEventBase
    {
        public OperatorId? Assignee { get; private set; }

        internal TicketAssignChangedEvent(TicketId ticketId, Initiator initiator, OperatorId? assignee) : base(
            ticketId, initiator)
        {
            Assignee = assignee;
        }
    }
}
