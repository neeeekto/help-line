using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class CancelRejectTicketCommand : TicketCommand
    {
        public TicketAuditId AuditId { get; private set; }

        public CancelRejectTicketCommand(TicketAuditId auditId)
        {
            AuditId = auditId;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new TicketCheckStatusRule(status => status.In(TicketStatusType.ForReject), ctx.Ticket.State));

            ctx.RiseEvent(new TicketApprovalStatusCanceledEvent(ctx.Ticket.Id, AuditId, ctx.Initiator));
            await ctx.Execute(new ChangeStatusTicketCommand(ctx.Ticket.State.FallbackStatus!));
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return VoidResult.Value;
        }
    }
}
