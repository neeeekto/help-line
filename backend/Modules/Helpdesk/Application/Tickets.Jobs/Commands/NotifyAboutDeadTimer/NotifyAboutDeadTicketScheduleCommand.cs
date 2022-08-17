using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Jobs.Commands.NotifyAboutDeadTimer
{
    internal class NotifyAboutDeadTicketScheduleCommand : CommandBase
    {
        public Guid ScheduleId { get; }

        [JsonConstructor]
        public NotifyAboutDeadTicketScheduleCommand(Guid scheduleId)
        {
            ScheduleId = scheduleId;
        }
    }
}
