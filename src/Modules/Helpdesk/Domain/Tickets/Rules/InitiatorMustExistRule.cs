using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class InitiatorMustExistRule : IBusinessRule
    {
        private Initiator _initiator;

        internal InitiatorMustExistRule(Initiator initiator)
        {
            _initiator = initiator;
        }

        public string Message => $"Initiator must be exist";
        public bool IsBroken() => _initiator == null;
    }
}
