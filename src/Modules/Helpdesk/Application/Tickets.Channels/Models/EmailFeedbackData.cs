using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models
{
    public class EmailFeedbackData : EmailMessageDataBase
    {
        public Guid FeedbackId { get; }
        public EmailFeedbackData(string ticketId, string projectId, string language, Guid feedbackId) : base(ticketId, projectId, language)
        {
            FeedbackId = feedbackId;
        }
    }
}
