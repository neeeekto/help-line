using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicketAtTime
{
    public class GetTicketAtTimeQuery : QueryBase<TicketView>
    {
        public string TicketId { get; }
        public DateTime Point { get; }

        public GetTicketAtTimeQuery(string ticketId, DateTime point)
        {
            TicketId = ticketId;
            Point = point;
        }
    }
}
