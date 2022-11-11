using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class RejectTicketCommand : TicketCommand
    {
        public Message? Message { get; private set; }

        public RejectTicketCommand(Message? message = null)
        {
            Message = message;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));

            if (ctx.Ticket.State.Status.In(TicketStatusType.New))
            {
                await ctx.Execute(new ChangeStatusTicketCommand(TicketStatus.Closed(TicketStatusType.Rejected)));
                ctx.RiseEvent(new TicketClosedEvent(ctx.Ticket.Id, ctx.Initiator));
            }
            else
            {
                if (ctx.Ticket.State.Status.In(TicketStatusType.ForReject))
                    return VoidResult.Value;

                ctx.CheckRule(new TicketShouldNotBeScheduledForRule(ctx.Ticket.State, TicketLifeCycleType.Feedback));
                ctx.CheckRule(new MessageCannotBeEmptyRule(Message, true));

                var newStatus = ctx.Ticket.State.Status.To(TicketStatusType.ForReject);
                await ctx.Execute(new ChangeStatusTicketCommand(newStatus));
                ctx.RiseEvent(new TicketApprovalStatusAddedEvent(ctx.Ticket.Id, ctx.Initiator, Message!,
                    new TicketAuditId(),
                    newStatus));
            }

            await ctx.Execute(new CheckAndAssignIfOperatorTicketCommand());
            await ctx.Execute(new CancelAllRemindersTicketCommand());
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return VoidResult.Value;
        }
    }
}
