using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ApproveRejectTicketCommand : TicketCommand
    {
        public TicketAuditId AuditId { get; private set; }

        public ApproveRejectTicketCommand(TicketAuditId auditId)
        {
            AuditId = auditId;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new TicketCheckStatusRule(status => status.In(TicketStatusType.ForReject), ctx.Ticket.State));
            await ctx.Execute(new CheckAndAssignIfOperatorTicketCommand());
            ctx.RiseEvent(new TicketApprovalStatusApprovedEvent(ctx.Ticket.Id, AuditId, ctx.Initiator));
            await ctx.Execute(new ChangeStatusTicketCommand(TicketStatus.Closed(TicketStatusType.Rejected)));
            ctx.RiseEvent(new TicketClosedEvent(ctx.Ticket.Id, ctx.Initiator));
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return VoidResult.Value;
        }
    }
}
