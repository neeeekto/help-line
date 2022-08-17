using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketMeta : ValueObject
    {
        public TicketId? FromTicketId { get; }
        public string Source { get; }
        public string? Platform { get; }

        public TicketMeta(string source)
        {
            FromTicketId = null;
            Source = source;
            Platform = null;
        }
        public TicketMeta(string source, TicketId? fromTicketId, string? platform)
        {
            FromTicketId = fromTicketId;
            Platform = platform;
            Source = source;
        }
    }
}
