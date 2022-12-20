using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetSchedules
{
    public class GetSchedulesByTicketQuery : QueryBase<IEnumerable<TicketSchedule>>
    {
        public string TicketId { get; }

        public GetSchedulesByTicketQuery(string ticketId)
        {
            TicketId = ticketId;
        }
    }
}
