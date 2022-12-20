using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ToggleUserChannelTicketCommand : TicketCommand
    {
        public UserId UserId { get; private set; }
        public bool Enabled { get; private set; }

        public ToggleUserChannelTicketCommand(UserId userId, bool enabled)
        {
            UserId = userId;
            Enabled = enabled;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new UserIdMustExistRule(UserId, ctx.Ticket.State.User.Ids.Select(x => x.UserId)));
            var current = ctx.Ticket.State.User.Ids.ToList();
            var idInfoInx = current.FindIndex(x => x.UserId == UserId);
            var idInfo = current[idInfoInx];
            ctx.CheckRule(new UserIdTypeMustBeAsRule(idInfo, UserIdType.Main));
            if (Enabled)
                await ctx.CheckRule(new UnsubscribeRule(ctx.Services.UnsubscribeManager, UserId,
                    ctx.Ticket.State.ProjectId));

            current[idInfoInx] = idInfo.Copy(enabled: Enabled);
            ctx.RiseEvent(new TicketUserIdsChangedEvent(ctx.Ticket.Id, ctx.Initiator, current));
            return VoidResult.Value;
        }
    }
}
