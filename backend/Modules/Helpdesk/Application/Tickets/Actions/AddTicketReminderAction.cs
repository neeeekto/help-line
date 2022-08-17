using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class AddTicketReminderAction : TicketActionBase
    {
        public TicketReminderDto Reminder { get; set; }

        public AddTicketReminderAction(TicketReminderDto reminder)
        {
            Reminder = reminder;
        }

        public AddTicketReminderAction()
        {
        }
    }
}
