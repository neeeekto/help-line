using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TicketMessageTemplateContent
    {
        public string Title { get; set; }
        public MessageDto Message { get; set; }
    }
}
