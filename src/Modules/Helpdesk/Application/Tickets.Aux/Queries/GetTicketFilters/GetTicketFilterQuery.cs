using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketFilters
{
    public class GetTicketFilterQuery : QueryBase<TicketSavedFilter>
    {
        public Guid FilterId { get; }

        public GetTicketFilterQuery(Guid filterId)
        {
            FilterId = filterId;
        }
    }
}
