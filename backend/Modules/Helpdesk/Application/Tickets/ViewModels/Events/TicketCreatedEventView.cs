using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketCreatedEventView : TicketEventView
    {
        public string ProjectId { get; internal set; }
        public string Language { get; internal set; }
        public IEnumerable<string> Tags { get; internal set; }
        public IEnumerable<UserIdInfoView> UserIds { get; internal set; }
        public IDictionary<string, string> UserMeta { get; internal set; }
        public TicketStatus Status { get; internal set; }
        public TicketPriority Priority { get; internal set; }
        public MessageView? Message { get; internal set; }
    }
}
