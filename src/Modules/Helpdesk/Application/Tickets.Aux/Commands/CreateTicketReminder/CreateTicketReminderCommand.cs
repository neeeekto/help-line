using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketReminder
{
    public class CreateTicketReminderCommand : CommandBase<Guid>
    {
        public string ProjectId { get; }

        public TicketReminderData Data { get; }

        public CreateTicketReminderCommand(string projectId, TicketReminderData data)
        {
            ProjectId = projectId;
            Data = data;
        }
    }
}
