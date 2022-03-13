using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketReminderShouldBeInStatusRule : IBusinessRule
    {
        private readonly TicketState _ticketState;
        private readonly TicketReminderId _reminderId;
        private readonly TicketReminderState.Statuses _status;

        internal TicketReminderShouldBeInStatusRule(TicketState ticketState, TicketReminderId reminderId, TicketReminderState.Statuses status)
        {
            _ticketState = ticketState;
            _reminderId = reminderId;
            _status = status;
        }

        public string Message => $"Reminder {_reminderId.Value.ToString()} should be in '{_status}' status";

        public bool IsBroken() => !_ticketState.Reminders.Any(x =>
            x.Reminder.Id == _reminderId && x.Status == _status);
    }
}
