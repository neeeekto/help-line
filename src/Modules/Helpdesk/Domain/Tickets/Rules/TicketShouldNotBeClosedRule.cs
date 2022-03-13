using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketShouldNotBeClosedRule : IBusinessRule
    {
        internal TicketShouldNotBeClosedRule(TicketState ticketState)
        {
            TicketState = ticketState;
        }

        private TicketState TicketState { get; }
        public string Message => "Ticket is closed";
        public bool IsBroken() => TicketState.Status.Kind == TicketStatusKind.Closed;
    }
}
