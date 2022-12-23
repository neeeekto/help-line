using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ReScheduleTimer
{
    public class ReScheduleTimerCommand : CommandBase
    {
        public Guid ScheduleId { get; }

        public ReScheduleTimerCommand(Guid scheduleId)
        {
            ScheduleId = scheduleId;
        }
    }
}
