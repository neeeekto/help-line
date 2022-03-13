using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ChangePriorityTicketCommand : TicketCommand
    {
        public TicketPriority Priority { get; private set; }

        public ChangePriorityTicketCommand(TicketPriority priority)
        {
            Priority = priority;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.RiseEvent(new TicketPriorityChangedEvent(ctx.Ticket.Id, ctx.Initiator, Priority));
            return VoidResult.Task;
        }
    }
}
