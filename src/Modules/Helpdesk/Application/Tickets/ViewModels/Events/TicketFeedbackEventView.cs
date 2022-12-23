using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketFeedbackEventView : TicketEventView
    {
        public Guid FeedbackId { get; internal set; }
    }
}
