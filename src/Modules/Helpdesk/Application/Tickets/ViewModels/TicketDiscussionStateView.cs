using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class TicketDiscussionStateView
    {
        public DateTime? LastReplyDate { get; internal set; }
        public MessageType LastMessageType { get; internal set; }
        public int IterationCount { get; internal set; }
        public bool HasAttachments { get; internal set; }

        public enum MessageType
        {
            Incoming,
            Outgoin
        }
    }
}
