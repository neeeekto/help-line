using System;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Jobs.Commands.ExecuteTicketSchedule
{
    internal class ExecuteTicketScheduleCommand : InternalCommandBase
    {
        public string TicketId { get; }
        public Guid ScheduleId { get; }

        [JsonConstructor]
        public ExecuteTicketScheduleCommand(Guid id, string ticketId, Guid scheduleId) : base(id)
        {
            TicketId = ticketId;
            ScheduleId = scheduleId;
        }


    }
}
