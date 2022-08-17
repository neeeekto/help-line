using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketShouldNotBeScheduledForRule : IBusinessRule
    {
        private readonly TicketState _ticketState;
        private readonly TicketLifeCycleType _lifeCycleType;

        internal TicketShouldNotBeScheduledForRule(TicketState ticketState, TicketLifeCycleType lifeCycleType)
        {
            _ticketState = ticketState;
            _lifeCycleType = lifeCycleType;
        }

        public string Message => $"The ticket is already scheduled for {_lifeCycleType.ToString()}";
        public bool IsBroken() => _ticketState.LifecycleStatus.ContainsKey(_lifeCycleType);
    }
}
