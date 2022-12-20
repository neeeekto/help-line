using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetSchedules
{
    public class GetSchedulesQuery : QueryBase<IEnumerable<TicketSchedule>>
    {
        public IEnumerable<TicketSchedule.Statuses> Statuses { get; }

        public GetSchedulesQuery(IEnumerable<TicketSchedule.Statuses> statuses)
        {
            Statuses = statuses;
        }
    }
}
