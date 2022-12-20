using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class TicketDelayConfigurationRequest
    {
        [Required]
        public Dictionary<TicketLifeCycleType, TimeSpan> LifeCycleDelay { get; set; }

        [Required]
        public TimeSpan InactivityDelay { get; set; }

        [Required]
        public TimeSpan FeedbackCompleteDelay { get; set; }
    }
}
