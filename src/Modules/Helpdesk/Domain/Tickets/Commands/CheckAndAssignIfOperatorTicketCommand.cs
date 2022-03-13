using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class CheckAndAssignIfOperatorTicketCommand : TicketCommand
    {
        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            if (ctx.Initiator is OperatorInitiator operatorInitiator && ctx.Ticket.State.AssignedTo == null)
            {
                await ctx.Execute(new AssignTicketCommand(operatorInitiator.OperatorId));
            }
            return VoidResult.Value;
        }
    }
}
