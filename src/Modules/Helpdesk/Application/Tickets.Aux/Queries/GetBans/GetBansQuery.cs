using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBans
{
    public class GetBansQuery : QueryBase<IEnumerable<Ban>>
    {
        public string ProjectId { get; }

        public GetBansQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
