using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class NoteRequest
    {
        public bool Secrete { get; set; }
        public MessageDto Message { get; set; }
    }
}
