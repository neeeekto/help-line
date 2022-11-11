using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class RemovePrivateNoteTicketCommand : TicketCommand
    {
        public TicketNoteId NoteId { get; private set; }

        public RemovePrivateNoteTicketCommand(TicketNoteId noteId)
        {
            NoteId = noteId;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketNoteMustExistRule(NoteId, ctx.Ticket.State));
            ctx.RiseEvent(new TicketNoteRemovedEvent(ctx.Ticket.Id, ctx.Initiator, NoteId));
            return VoidResult.Task;
        }
    }
}
