using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketMustHaveNotResolveReminderRule : IBusinessRule
    {
        private readonly IEnumerable<TicketReminderState> _reminders;

        internal TicketMustHaveNotResolveReminderRule(IEnumerable<TicketReminderState> reminders)
        {
            _reminders = reminders;
        }

        public string Message => "Ticket has active resolve reminder";

        public bool IsBroken() => _reminders.Where(x => x.Status == TicketReminderState.Statuses.Scheduled)
            .Any(x => HasResolveReminder(x.Reminder));

        private bool HasResolveReminder(TicketReminder reminder) => reminder switch
        {
            TicketSequentialReminder sequential => HasResolveReminder(sequential.Next),
            TicketFinalReminder final => final.Resolve,
            _ => false
        };
    }
}
