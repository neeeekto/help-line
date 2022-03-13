using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class InitiatorShouldNotBeRule<T> : IBusinessRule
        where T : Initiator
    {
        private readonly Initiator _initiator;

        internal InitiatorShouldNotBeRule(Initiator initiator)
        {
            _initiator = initiator;
        }

        public string Message => $"The initiator should not be {typeof(T).Name}";
        public bool IsBroken() => (_initiator is T);
    }
}
