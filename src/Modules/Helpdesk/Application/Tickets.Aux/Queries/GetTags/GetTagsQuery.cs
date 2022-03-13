using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTags
{
    public class GetTagsQuery : QueryBase<IEnumerable<Tag>>
    {
        public string ProjectId { get; }
        public bool OnlyEnabled { get; }

        public GetTagsQuery(string projectId, bool onlyEnabled = false)
        {
            ProjectId = projectId;
            OnlyEnabled = onlyEnabled;
        }
    }
}
