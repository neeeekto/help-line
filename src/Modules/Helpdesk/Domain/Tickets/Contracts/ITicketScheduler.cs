using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    // TODO: Да, мальчик мой, это я...
    // Если думаешь какого хера юзается модель шедулинга, то подумай - а как выбирать в эвент модели тикеты на закрытие? Базу заеп.м эвенты перебирать)
    public interface ITicketScheduler
    {
        Task Schedule(DateTime executionDate, TicketId ticketId, ScheduleId scheduleId);
        Task Prolong(DateTime executionDate, TicketId ticketId, ScheduleId scheduleId);
        Task Cancel(ScheduleId scheduleId);
    }
}
