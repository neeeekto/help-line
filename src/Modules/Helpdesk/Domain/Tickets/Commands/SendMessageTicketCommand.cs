using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class SendMessageTicketCommand : TicketCommand<TicketOutgoingMessageId>
    {
        public Message Message { get; private set; }

        public SendMessageTicketCommand(Message message)
        {
            Message = message;
        }

        internal override async Task<TicketOutgoingMessageId> Execute(CommandContext ctx)
        {
            var messageId = new TicketOutgoingMessageId();
            await ctx.Services.MessageDispatcher.Enqueue(ctx.Ticket.Id, messageId, Message,
                ctx.Ticket.State.User.Channels, ctx.Ticket.State.ProjectId);
            ctx.RiseEvent(new TicketOutgoingMessageAddedEvent(ctx.Ticket.Id, ctx.Initiator, Message,
                ctx.Ticket.State.User.Channels,
                messageId));
            return messageId;
        }
    }
}
