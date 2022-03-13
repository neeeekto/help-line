using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketShouldBeScheduledForRule : IBusinessRule
    {
        private readonly TicketState _ticketState;
        private readonly TicketLifeCycleType _lifeCycleType;

        internal TicketShouldBeScheduledForRule(TicketState ticketState, TicketLifeCycleType lifeCycleType)
        {
            _ticketState = ticketState;
            _lifeCycleType = lifeCycleType;
        }

        public string Message => $"The ticket is not scheduled for {_lifeCycleType.ToString()}";
        public bool IsBroken() => !_ticketState.LifecycleStatus.ContainsKey(_lifeCycleType);
    }
}
