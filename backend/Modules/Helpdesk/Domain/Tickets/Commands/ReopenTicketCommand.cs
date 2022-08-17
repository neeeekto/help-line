using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Extends;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ReopenTicketCommand : TicketCommand
    {
        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new TicketCheckStatusRule(
                status => status.In(TicketStatusType.Resolved, TicketStatusType.ForReject), ctx.Ticket.State));
            // CheckRule(new TicketHasCorrectStatusRule(status => status.In(TicketStatusType.Resolved, TicketStatusType.Rejected), Ticket.State));
            ctx.PublishStatusChangedEvent(ctx.Ticket.State.FallbackStatus!);
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return VoidResult.Value;
        }
    }
}
