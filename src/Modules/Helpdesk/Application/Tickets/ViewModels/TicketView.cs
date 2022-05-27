using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class TicketView
    {
        public string Id { get; internal set; }
        public string ProjectId { get; internal set; }
        public IEnumerable<string> Tags { get; internal set; }
        public string Language { get; internal set; }
        public TicketStatusView Status { get; internal set; }
        public TicketPriority Priority { get; internal set; }
        public bool HardAssigment { get; internal set; }
        public Guid? AssignedTo { get; internal set; }
        public DateTime CreateDate { get; internal set; }
        public DateTime DateOfLastStatusChange { get; internal set; }
        public TicketDiscussionStateView DiscussionState { get; internal set; }
        public string Title { get; internal set; } // First message text

        public IEnumerable<FeedbackReviewView> Feedbacks { get; internal set; }
        public IEnumerable<UserIdInfoView> UserIds { get; internal set; }
        public Dictionary<string, string> UserMeta { get; internal set; }
        public IEnumerable<TicketEventView> Events { get; internal set; }
        public TicketMetaView Meta { get; internal set; }

        public TicketView()
        {
            Feedbacks = new List<FeedbackReviewView>();
            UserIds = new List<UserIdInfoView>();
            UserMeta = new Dictionary<string, string>();
            Events = new List<TicketEventView>();
            DiscussionState = new TicketDiscussionStateView();
        }
    }
}
