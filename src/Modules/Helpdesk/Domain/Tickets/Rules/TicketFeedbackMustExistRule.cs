using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketFeedbackMustExistRule : IBusinessRule
    {
        private readonly TicketFeedbackId _feedbackId;
        private readonly TicketState _ticketState;

        public TicketFeedbackMustExistRule(TicketFeedbackId feedbackId, TicketState ticketState)
        {
            _feedbackId = feedbackId;
            _ticketState = ticketState;
        }

        public string Message => $"Feedback '{_feedbackId}' was not sent by ticket";
        public bool IsBroken() => _ticketState.Feedbacks.All(x => x.Key != _feedbackId);
    }
}
