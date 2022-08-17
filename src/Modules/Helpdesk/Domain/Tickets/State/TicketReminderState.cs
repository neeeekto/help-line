namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketReminderState
    {
        public ScheduleId ScheduleId { get; }
        public TicketReminder Reminder { get;}
        public Statuses Status { get; internal set; }
        public Initiator Initiator { get; }

        internal TicketReminderState(ScheduleId scheduleId, TicketReminder reminder, Statuses status, Initiator initiator)
        {
            ScheduleId = scheduleId;
            Reminder = reminder;
            Status = status;
            Initiator = initiator;
        }


        public enum Statuses
        {
            Scheduled = -1,
            Executed = 1,
            Canceled = 0,
        }
    }
}
