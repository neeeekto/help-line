using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReopenConditions
{
    public class GetTicketReopenConditionsQuery : QueryBase<IEnumerable<TicketReopenCondition>>
    {
        public string ProjectId { get; }

        public GetTicketReopenConditionsQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
