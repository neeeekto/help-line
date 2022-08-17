using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketMessageTemplates
{
    public class GetTicketMessageTemplatesQuery : QueryBase<IEnumerable<TicketMessageTemplate>>
    {
        public string ProjectId { get; }

        public GetTicketMessageTemplatesQuery(string projectId)
        {
            ProjectId = projectId;
        }


    }
}
