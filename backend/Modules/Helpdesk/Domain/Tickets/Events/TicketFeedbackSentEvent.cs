using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketFeedbackSentEvent : TicketEventBase
    {
        public TicketFeedbackId FeedbackId { get; private set; }
        internal TicketFeedbackSentEvent(TicketId ticketId, TicketFeedbackId feedbackId, Initiator initiator) : base(ticketId, initiator)
        {
            FeedbackId = feedbackId;
        }
    }
}
