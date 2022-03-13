using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket
{
    public class GetTicketQuery : QueryBase<TicketView>
    {
        public string TicketId { get; }

        public GetTicketQuery(string ticketId)
        {
            TicketId = ticketId;
        }


    }
}
