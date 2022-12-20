using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.DeleteScheduleTimer
{
    public class DeleteScheduleTimerCommand : CommandBase
    {
        public Guid ScheduleId { get; }

        public DeleteScheduleTimerCommand(Guid scheduleId)
        {
            ScheduleId = scheduleId;
        }
    }
}
