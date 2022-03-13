using System;
using System.Collections.ObjectModel;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetTicketDelayConfiguration
{
    public class SetTicketDelayConfigurationCommand : CommandBase
    {
        public string ProjectId { get; }
        public ReadOnlyDictionary<TicketLifeCycleType, TimeSpan> LifeCycleDelay { get; }
        public TimeSpan InactivityDelay { get; }
        public TimeSpan FeedbackCompleteDelay { get; }

        public SetTicketDelayConfigurationCommand(string projectId, ReadOnlyDictionary<TicketLifeCycleType, TimeSpan> lifeCycleDelay, TimeSpan inactivityDelay, TimeSpan feedbackCompleteDelay)
        {
            ProjectId = projectId;
            LifeCycleDelay = lifeCycleDelay;
            InactivityDelay = inactivityDelay;
            FeedbackCompleteDelay = feedbackCompleteDelay;
        }

        [JsonConstructor]
        internal SetTicketDelayConfigurationCommand(Guid id, string projectId, ReadOnlyDictionary<TicketLifeCycleType, TimeSpan> lifeCycleDelay, TimeSpan inactivityDelay, TimeSpan feedbackCompleteDelay) : base(id)
        {
            ProjectId = projectId;
            LifeCycleDelay = lifeCycleDelay;
            InactivityDelay = inactivityDelay;
            FeedbackCompleteDelay = feedbackCompleteDelay;
        }



    }
}
