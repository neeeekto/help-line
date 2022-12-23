using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class RetryOutgoingMessageTicketCommand : TicketCommand
    {
        public TicketOutgoingMessageId MessageId { get; private set; }
        public UserId UserId { get; private set; }

        public RetryOutgoingMessageTicketCommand(TicketOutgoingMessageId messageId, UserId userId)
        {
            MessageId = messageId;
            UserId = userId;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new TicketOutgoingMessageMustExistRule(ctx.Ticket.State.OutgoingMessages, MessageId));

            var message = ctx.Ticket.State.OutgoingMessages.First(x => x.Id == MessageId);
            ctx.CheckRule(new UserIdMustExistRule(UserId, ctx.Ticket.State.User.Channels.Select(x => x.UserId)));
            ctx.CheckRule(
                new TicketOutgoingMessageMustNotSendingRule(ctx.Ticket.State.OutgoingMessages, MessageId, UserId));

            await ctx.Services.MessageDispatcher.Enqueue(ctx.Ticket.Id, MessageId, message.Message,
                ctx.Ticket.State.User.Channels, ctx.Ticket.State.ProjectId);
            ctx.RiseEvent(new TicketMessageStatusChangedEvent(ctx.Ticket.Id, ctx.Initiator, MessageId, UserId,
                MessageStatus.Sending));
            return VoidResult.Value;
        }
    }
}
