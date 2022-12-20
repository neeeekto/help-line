using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketMustBeAssignedToOperatorRule : IBusinessRule
    {
        private readonly TicketState _state;

        internal TicketMustBeAssignedToOperatorRule(TicketState state)
        {
            _state = state;
        }

        public string Message => "Ticket is not assigned to operator";
        public bool IsBroken() => _state.AssignedTo == null;
    }
}
