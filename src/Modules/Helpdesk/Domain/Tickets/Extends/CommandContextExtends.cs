using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Extends
{
    internal static class CommandContextExtends
    {
        public static void PublishStatusChangedEvent(this CommandContext ctx, TicketStatus status, Initiator? initiator = null)
        {
            if (ctx.Ticket.State.Status != status)
            {
                ctx.RiseEvent(new TicketStatusChangedEvent(ctx.Ticket.Id, initiator ?? ctx.Initiator, status));
            }
        }
    }
}
