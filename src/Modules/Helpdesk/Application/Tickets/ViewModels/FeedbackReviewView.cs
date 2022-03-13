using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class FeedbackReviewView
    {
        public DateTime DateTime { get; internal set; }
        public Guid FeedbackId { get; internal set; }
        public int Score { get; internal set; }
        public string? Message { get; internal set; }
        public bool? Solved { get; internal set; }
        public IDictionary<string, int>? OptionalScores { get; internal set; }
    }
}
