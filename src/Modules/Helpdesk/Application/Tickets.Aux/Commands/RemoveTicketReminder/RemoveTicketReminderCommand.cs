using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketReminder
{
    public class RemoveTicketReminderCommand : CommandBase
    {
        public Guid ReminderId { get; }

        public RemoveTicketReminderCommand(Guid reminderId)
        {
            ReminderId = reminderId;
        }


    }
}
