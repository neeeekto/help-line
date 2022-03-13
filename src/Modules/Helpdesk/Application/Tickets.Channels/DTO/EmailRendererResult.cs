namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.DTO
{
    public class EmailRendererResult
    {
        public string Body { get; set; }
        public string Subject { get; set; }

        public EmailRendererResult(string body, string subject)
        {
            Body = body;
            Subject = subject;
        }
    }
}
