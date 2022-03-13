using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.UpdateTicketReminder
{
    public class UpdateTicketReminderCommand : CommandBase
    {
        public Guid ReminderId { get; }
        public TicketReminderData Data { get; }

        public UpdateTicketReminderCommand(Guid reminderId, TicketReminderData data)
        {
            ReminderId = reminderId;
            Data = data;
        }
    }
}
