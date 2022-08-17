using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ImmediateSendFeedbackTicketCommand : TicketCommand<TicketFeedbackId?>
    {
        internal override async Task<TicketFeedbackId?> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldBeScheduledForRule(ctx.Ticket.State, TicketLifeCycleType.Feedback));

            var scheduleId = ctx.Ticket.State.LifecycleStatus[TicketLifeCycleType.Feedback];
            await ctx.Services.Scheduler.Cancel(scheduleId);
            ctx.RiseEvent(new TicketLifecycleCanceledEvent(ctx.Ticket.Id, ctx.Initiator, scheduleId, TicketLifeCycleType.Feedback));
            return await ctx.Execute(new SendFeedbackTicketCommand());
        }
    }
}
