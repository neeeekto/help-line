using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SaveFeedback
{
    public class SaveFeedbackCommand : CommandBase
    {
        public string TicketId { get; }
        public Guid FeedbackId { get; }
        public TicketFeedbackDto Feedback { get; }
        public string UserId { get; }

        public SaveFeedbackCommand(string ticketId, Guid feedbackId, TicketFeedbackDto feedback, string userId)
        {
            TicketId = ticketId;
            FeedbackId = feedbackId;
            Feedback = feedback;
            UserId = userId;
        }
    }
}
