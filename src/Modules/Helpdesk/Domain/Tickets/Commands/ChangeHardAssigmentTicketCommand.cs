using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ChangeHardAssigmentTicketCommand : TicketCommand
    {
        public bool HardAssigment { get; private set; }

        public ChangeHardAssigmentTicketCommand(bool hardAssigment)
        {
            HardAssigment = hardAssigment;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));

            if (ctx.Ticket.State.HardAssigment != HardAssigment)
            {
                if (HardAssigment)
                    ctx.CheckRule(new TicketMustBeAssignedToOperatorRule(ctx.Ticket.State));
                ctx.RiseEvent(new TicketAssingmentBindingChangedEvent(ctx.Ticket.Id, ctx.Initiator, HardAssigment));
            }

            return VoidResult.Task;
        }
    }
}
