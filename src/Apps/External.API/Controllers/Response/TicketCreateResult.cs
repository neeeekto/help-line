namespace HelpLine.Apps.External.API.Controllers.Response
{
    public class TicketCreateResult
    {
        public bool Success { get; }
        public string? TicketId { get;  }

        public TicketCreateResult(bool success, string? ticketId = null)
        {
            Success = success;
            TicketId = ticketId;
        }
    }
}
