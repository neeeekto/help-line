using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ChangePrivateNoteTagsTicketCommand : TicketCommand
    {
        public TicketNoteId NoteId { get; private set; }
        public IEnumerable<string> Tags { get; private set; }

        public ChangePrivateNoteTagsTicketCommand(TicketNoteId noteId, IEnumerable<string> tags)
        {
            NoteId = noteId;
            Tags = tags;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketNoteMustExistRule(NoteId, ctx.Ticket.State));

            var note = ctx.Ticket.State.Notes[NoteId];
            ctx.RiseEvent(new TicketNotePostedEvent(ctx.Ticket.Id, ctx.Initiator, NoteId, note.Message, Tags));
            return VoidResult.Task;
        }
    }
}
