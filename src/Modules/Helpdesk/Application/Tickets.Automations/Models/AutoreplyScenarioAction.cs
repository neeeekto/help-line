using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models
{
    public class AutoreplyScenarioAction
    {
        public LocalizeDictionary<MessageDto> Message { get; set; }
        public bool Resolve { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public TicketReminderDto? Reminder { get; set; }
    }
}
