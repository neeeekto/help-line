using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class AddOutgoingMessageTicketCommand : TicketCommand<TicketOutgoingMessageId>
    {
        public Message Message { get; private set; }

        public AddOutgoingMessageTicketCommand(Message message)
        {
            Message = message;
        }

        internal override async Task<TicketOutgoingMessageId> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new MessageCannotBeEmptyRule(Message));
            ctx.CheckRule(new UserChannelsCannotBeEmptyRule(ctx.Ticket.State.User.Channels));

            var messageId = await ctx.Execute(new SendMessageTicketCommand(Message));
            await ctx.Execute(new ChangeStatusTicketCommand(ctx.Ticket.State.Status.To(TicketStatusType.Answered)));
            await ctx.Execute(new CancelAllRemindersTicketCommand());

            if (ctx.Initiator is OperatorInitiator operatorInitiator && !ctx.Ticket.State.HardAssigment)
                await ctx.Execute(new AssignTicketCommand(operatorInitiator.OperatorId));
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return messageId;
        }
    }
}
