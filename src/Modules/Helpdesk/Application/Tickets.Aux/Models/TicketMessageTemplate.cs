using System;
using System.Collections.ObjectModel;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TicketMessageTemplate
    {
        public Guid Id { get; internal set; }
        public string ProjectId { get; internal set; }
        public int Order { get; internal set; }
        public DateTime ModifyDate { get; internal set; } // for client
        public string? Group { get; internal set; } // only for client
        public ReadOnlyDictionary<string, TicketMessageTemplateContent> Content { get; internal set; }
    }
}
