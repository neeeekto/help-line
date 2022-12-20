namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models
{
    public abstract class EmailMessageDataBase
    {
        public string TicketId { get; }
        public string ProjectId { get; }
        public string Language { get; }

        public EmailMessageDataBase(string ticketId, string projectId, string language)
        {
            TicketId = ticketId;
            ProjectId = projectId;
            Language = language;
        }
    }
}
