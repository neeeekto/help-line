using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class RemoveUserIdTicketCommand : TicketCommand
    {
        public UserId UserId { get; private set; }

        public RemoveUserIdTicketCommand(UserId userId)
        {
            UserId = userId;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new UserIdMustExistRule(UserId, ctx.Ticket.State.User.Ids.Select(x => x.UserId)));
            var next = ctx.Ticket.State.User.Ids.ToList().Where(x => x.UserId != UserId);
            ctx.RiseEvent(new TicketUserIdsChangedEvent(ctx.Ticket.Id, ctx.Initiator, next));
            return VoidResult.Task;
        }
    }
}
