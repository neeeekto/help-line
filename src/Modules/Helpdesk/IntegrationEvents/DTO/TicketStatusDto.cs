namespace HelpLine.Modules.Helpdesk.IntegrationEvents.DTO
{
    public enum Types
    {
        New, // opened, pending
        Answered, // opened, pending
        AwaitingReply, // opened, pending
        Resolved, // opened, closed
        Rejected, // closed
        ForReject, // opened // TODO: подумать над переноси ForReject как Rejected + Open состояние
    }

    public enum Kinds
    {
        Opened,
        Closed,
        Pending
    }

    public class TicketStatusDto
    {
        public Kinds Kind { get; set; }
        public Types Type { get; set; }
    }
}
