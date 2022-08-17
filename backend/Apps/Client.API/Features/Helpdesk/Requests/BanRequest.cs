using System;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class BanRequest
    {
        public Ban.Parameters Parameter { get; set; }
        public string Value { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }
}
