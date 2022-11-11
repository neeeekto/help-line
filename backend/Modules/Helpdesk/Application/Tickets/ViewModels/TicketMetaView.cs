using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class TicketMetaView
    {
        public string? FromTicket { get; internal set; }
        public string Source { get; internal set; }
        public string? Platform { get; internal set; }

        internal TicketMetaView(TicketMeta meta)
        {
            FromTicket = meta.FromTicketId?.Value;
            Source = meta.Source;
            Platform = meta.Platform;
        }
    }
}
