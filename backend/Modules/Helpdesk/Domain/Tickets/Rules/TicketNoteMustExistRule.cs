using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketNoteMustExistRule : IBusinessRule
    {
        private readonly TicketNoteId _noteId;
        private readonly TicketState _ticketState;

        internal TicketNoteMustExistRule(TicketNoteId noteId, TicketState ticketState)
        {
            _noteId = noteId;
            _ticketState = ticketState;
        }

        public string Message => $"Note {_noteId.Value} not exist";
        public bool IsBroken() => _ticketState.Notes.All(x => x.Key != _noteId);
    }
}
