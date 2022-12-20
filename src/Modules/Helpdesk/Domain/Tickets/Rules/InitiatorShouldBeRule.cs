using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class InitiatorShouldBeRule<T> : IBusinessRule where T : Initiator
    {
        private readonly Initiator _initiator;

        internal InitiatorShouldBeRule(Initiator initiator)
        {
            _initiator = initiator;
        }

        public string Message => $"The initiator must be {typeof(T).Name}, not the {_initiator.GetType().Name}";
        public bool IsBroken() => !(_initiator is T);
    }
}
