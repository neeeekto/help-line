using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ChangeUserMetaTicketCommand : TicketCommand
    {
        public UserMeta Meta { get; private set; }

        public ChangeUserMetaTicketCommand(UserMeta meta)
        {
            Meta = meta;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.RiseEvent(new TicketUserMetaChangedEvent(ctx.Ticket.Id, ctx.Initiator, Meta));
            return VoidResult.Task;
        }
    }
}
