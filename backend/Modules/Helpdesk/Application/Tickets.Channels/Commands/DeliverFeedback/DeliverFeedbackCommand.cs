using System;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.DeliverFeedback
{
    internal class DeliverFeedbackCommand : InternalCommandBase
    {
        public string TicketId { get; }
        public Guid FeedbackId { get; }
        public string ProjectId { get; }
        public string Channel { get; }
        public string UserId { get; }


        [JsonConstructor]
        public DeliverFeedbackCommand(Guid id, string ticketId, Guid feedbackId, string projectId, string channel,
            string userId) : base(id)
        {
            TicketId = ticketId;
            FeedbackId = feedbackId;
            ProjectId = projectId;
            Channel = channel;
            UserId = userId;
        }


    }
}
