using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetUnsubscribes
{
    public class GetUnsubscribesQuery : QueryBase<IEnumerable<Unsubscribe>>
    {
        public string ProjectId { get; }

        public GetUnsubscribesQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
