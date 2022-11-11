using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    /// <summary>
    /// Проверка на то, что фидбек не просрочен на модификацию
    /// </summary>
    public class TicketFeedbackMustBeAvailableForModificationRule : IBusinessRuleAsync
    {
        private readonly ITicketConfigurations _configurations;
        private readonly TicketState _ticketState;
        private readonly TicketFeedbackId _ticketFeedbackId;

        public TicketFeedbackMustBeAvailableForModificationRule(ITicketConfigurations configurations, TicketState ticketState,
            TicketFeedbackId ticketFeedbackId)
        {
            _configurations = configurations;
            _ticketState = ticketState;
            _ticketFeedbackId = ticketFeedbackId;
        }

        public string Message => $"Feedback change timed out";

        public async Task<bool> IsBroken()
        {
            if (_ticketState.Feedbacks.TryGetValue(_ticketFeedbackId, out var date))
            {
                // Не заполнен
                if (date == null) return false;

                var delay = await _configurations.GetFeedbackCompleteDelay(_ticketState.ProjectId);
                return DateTime.UtcNow - date > delay;
            }

            // Нет фидбека
            return true;
        }
    }
}
