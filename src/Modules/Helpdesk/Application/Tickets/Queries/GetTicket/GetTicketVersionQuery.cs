using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;

public class GetTicketVersionQuery : QueryBase<int>
{
    public string TicketId { get; }

    public GetTicketVersionQuery(string ticketId)
    {
        TicketId = ticketId;
    }
}