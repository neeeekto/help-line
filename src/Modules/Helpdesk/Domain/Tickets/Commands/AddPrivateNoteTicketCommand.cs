using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class AddPrivateNoteTicketCommand : TicketCommand<TicketNoteId>
    {
        public Message Message { get; private set; }
        public IEnumerable<string>? Tags { get; private set; }

        public AddPrivateNoteTicketCommand(Message message, IEnumerable<string>? tags = null)
        {
            Message = message;
            Tags = tags;
        }

        internal override Task<TicketNoteId> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new MessageCannotBeEmptyRule(Message));
            var noteId = new TicketNoteId();
            ctx.RiseEvent(new TicketNotePostedEvent(ctx.Ticket.Id, ctx.Initiator, noteId, Message, Tags ?? System.Array.Empty<string>()));
            return Task.FromResult(noteId);
        }
    }
}
