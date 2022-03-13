namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Extends
{
    internal static class TicketExtends
    {
        public static bool IsAssigned(this Ticket ticket) => ticket.State.AssignedTo != null;
    }
}
