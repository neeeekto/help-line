using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class CancelTicketReminderAction : TicketActionBase
    {
        public Guid ReminderId { get; set; }

        public CancelTicketReminderAction()
        {
        }

        public CancelTicketReminderAction(Guid reminderId)
        {
            ReminderId = reminderId;
        }
    }
}
