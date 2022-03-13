using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class UnsubscribeTicketCommand : TicketCommand
    {
        public UserId UserId { get; private set; }
        public string Message { get; private set; }

        public UnsubscribeTicketCommand(UserId userId, string message)
        {
            UserId = userId;
            Message = message;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            var initiator = new UserInitiator(UserId);
            ctx.CheckRule(new UserIdMustExistRule(UserId, ctx.Ticket.State.User.Ids.Select(x => x.UserId)));
            var current = ctx.Ticket.State.User.Ids.ToList();
            var idInfoInx = current.FindIndex(x => x.UserId == UserId);
            var idInfo = current[idInfoInx];
            ctx.CheckRule(new UserIdTypeMustBeAsRule(idInfo, UserIdType.Main));
            current[idInfoInx] = idInfo.Copy(enabled: false);
            ctx.RiseEvent(new TicketUserIdsChangedEvent(ctx.Ticket.Id, initiator, current));
            ctx.RiseEvent(new TicketUserUnsubscribedEvent(ctx.Ticket.Id, UserId, Message, initiator));
            return VoidResult.Task;
        }
    }
}
