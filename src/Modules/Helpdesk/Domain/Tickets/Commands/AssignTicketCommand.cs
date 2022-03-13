using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class AssignTicketCommand : TicketCommand
    {
        public OperatorId OperatorId { get; private set; }

        public AssignTicketCommand(OperatorId operatorId)
        {
            OperatorId = operatorId;
        }

        public AssignTicketCommand(OperatorInitiator initiator)
        {
            OperatorId = initiator.OperatorId;
        }

        internal override Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));

            if (ctx.Ticket.State.AssignedTo != OperatorId)
                ctx.RiseEvent(new TicketAssignChangedEvent(ctx.Ticket.Id, ctx.Initiator, OperatorId));
            return VoidResult.Task;
        }
    }
}
