using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class InitiatorMustBeSameRule : IBusinessRule
    {
        private readonly Initiator _first;
        private readonly Initiator _second;

        internal InitiatorMustBeSameRule(Initiator first, Initiator second)
        {
            _first = first;
            _second = second;
        }

        public string Message => "Initiators are not identity";
        public bool IsBroken() => _first != _second;
    }
}
