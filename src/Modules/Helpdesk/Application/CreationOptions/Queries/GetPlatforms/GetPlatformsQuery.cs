using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetPlatforms
{
    public class GetPlatformsQuery : QueryBase<IEnumerable<Platform>>
    {
        public string ProjectId { get; }

        public GetPlatformsQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
