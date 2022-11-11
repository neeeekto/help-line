using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketFeedbackAddedEvent : TicketEventBase
    {
        public TicketFeedbackId FeedbackId { get; private set; }
        public TicketFeedback Feedback { get; private set; }

        internal TicketFeedbackAddedEvent(TicketId ticketId, Initiator initiator, TicketFeedbackId feedbackId, TicketFeedback feedback) : base(ticketId, initiator)
        {
            FeedbackId = feedbackId;
            Feedback = feedback;
        }
    }
}
