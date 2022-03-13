using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Extends;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class ChangeStatusTicketCommand : TicketCommand
    {
        public TicketStatus Status { get; private set; }

        public ChangeStatusTicketCommand(TicketStatus status)
        {
            Status = status;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new TicketMustHaveCorrectStatusRule(ctx.Ticket.State.Status, Status));
            ctx.PublishStatusChangedEvent(Status);
            return VoidResult.Task;
        }
    }
}
