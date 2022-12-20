using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketOutgoingMessageEventView : TicketEventView
    {
        public Guid MessageId { get; internal set; }
        public MessageView Message { get; internal set; }
        public IEnumerable<RecipientView> Recipients { get; internal set; }
    }
}
