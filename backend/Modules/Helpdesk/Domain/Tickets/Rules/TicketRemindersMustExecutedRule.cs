using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketRemindersMustExecutedRule : IBusinessRule
    {
        private readonly IEnumerable<TicketReminderState> _ticketReminders;

        internal TicketRemindersMustExecutedRule(IEnumerable<TicketReminderState> ticketReminders)
        {
            _ticketReminders = ticketReminders;
        }

        public string Message => $"Ticket has reminders";
        public bool IsBroken() => _ticketReminders.Any(x => x.Status == TicketReminderState.Statuses.Scheduled);
    }
}
