using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class MessageTemplateRequest
    {
        public Dictionary<string, TicketMessageTemplateContent?> Contents { get; set; }
        public string? Group { get; set; }
    }
}
