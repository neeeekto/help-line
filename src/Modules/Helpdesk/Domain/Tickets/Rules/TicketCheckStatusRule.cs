using System;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketCheckStatusRule : IBusinessRule
    {
        private readonly Func<TicketStatus, bool> _cheker;
        private readonly TicketState _state;

        internal TicketCheckStatusRule(Func<TicketStatus, bool> cheker, TicketState state)
        {
            _cheker = cheker;
            _state = state;
        }

        public string Message => $"Ticket has incorrect status: {_state.Status}";
        public bool IsBroken() => !_cheker(_state.Status);
    }
}
