using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class AddUserIdTicketCommand : TicketCommand
    {
        public UserId UserId { get; private set; }
        public UserChannelState State { get; private set; }
        public UserIdType Type { get; private set; }

        public AddUserIdTicketCommand(UserId userId, UserChannelState state, UserIdType type)
        {
            UserId = userId;
            State = state;
            Type = type;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new UserIdMusNotExistRule(UserId, ctx.Ticket.State.User.Ids.Select(x => x.UserId)));
            var useForDiscussion = Type == UserIdType.Main && State.Enabled;
            if (useForDiscussion)
                await ctx.CheckRule(new UnsubscribeRule(ctx.Services.UnsubscribeManager, UserId,
                    ctx.Ticket.State.ProjectId));
            // Only main id may be used for disscussion
            var newId = new UserIdInfo(UserId, State.Channel, Type, useForDiscussion);
            var next = ctx.Ticket.State.User.Ids.Concat(new[] {newId});
            ctx.RiseEvent(new TicketUserIdsChangedEvent(ctx.Ticket.Id, ctx.Initiator, next));
            return VoidResult.Value;
        }
    }
}
