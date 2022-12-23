using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class AddMessageStatusTicketCommand : TicketCommand
    {
        public TicketOutgoingMessageId MessageId { get; private set; }
        public UserId UserId { get; private set; }
        public MessageStatus Status { get; private set; }
        public string? Reason { get; private set; }
        public MessageMeta? Meta { get; private set; }

        public AddMessageStatusTicketCommand(TicketOutgoingMessageId messageId, UserId userId, MessageStatus status, string? reason = null, 
            MessageMeta? meta = null)
        {
            MessageId = messageId;
            UserId = userId;
            Status = status;
            Reason = reason;
            Meta = meta;
        }

        internal override Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketOutgoingMessageMustExistRule(ctx.Ticket.State.OutgoingMessages, MessageId));

            var message = ctx.Ticket.State.OutgoingMessages.First(x => x.Id == MessageId);
            ctx.CheckRule(new UserIdMustExistRule(UserId, message.Statuses.Select(x => x.Key)));

            ctx.RiseEvent(new TicketMessageStatusChangedEvent(ctx.Ticket.Id, new SystemInitiator(), MessageId, UserId, Status, Reason));
            return VoidResult.Task;
        }
    }
}
