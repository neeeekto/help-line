using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketReminderScheduledEvent : TicketScheduleEventBase
    {
        public TicketReminder Reminder { get; private set; }
        public DateTime ExecutionDate { get; private set; }

        internal TicketReminderScheduledEvent(TicketId ticketId, Initiator initiator, ScheduleId scheduleId, TicketReminder reminder, DateTime executionDate) : base(ticketId, initiator, scheduleId)
        {
            Reminder = reminder;
            ExecutionDate = executionDate;
        }
    }
}
