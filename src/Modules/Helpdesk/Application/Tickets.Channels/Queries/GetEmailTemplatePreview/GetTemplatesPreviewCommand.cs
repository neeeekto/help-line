using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetEmailTemplatePreview
{
    public abstract class GetEmailTemplatePreviewQueryBase : QueryBase<EmailRendererResult>
    {
        public string TicketId { get; }

        protected GetEmailTemplatePreviewQueryBase(string ticketId)
        {
            TicketId = ticketId;
        }
    }

    public class GetEmailFeedbackTemplatePreviewQuery : GetEmailTemplatePreviewQueryBase
    {
        public Guid FeedbackId { get; }

        public GetEmailFeedbackTemplatePreviewQuery(string ticketId, Guid feedbackId) : base(ticketId)
        {
            FeedbackId = feedbackId;
        }
    }

    public class GetEmailMessageTemplatePreviewQuery : GetEmailTemplatePreviewQueryBase
    {
        public GetEmailMessageTemplatePreviewQuery(string ticketId) : base(ticketId)
        {
        }
    }
}
