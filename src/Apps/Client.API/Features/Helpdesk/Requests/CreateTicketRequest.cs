using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class CreateTicketRequest
    {
        public string Language { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public Dictionary<string, string> Channels { get; set; } // {userId: channelKey}
        public Dictionary<string, string> UserMeta { get; set; } // {key: val} es: {device: phone, os: android 8.1}
        public MessageDto? Message { get; set; }
        public string? FromTicket { get; set; }
    }
}
