using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTagsDescriptions
{
    public class GetTagsDescriptionsQuery : QueryBase<IEnumerable<TagsDescription>>
    {
        public string ProjectId { get; }
        public IEnumerable<Guid> Audience { get; }
        public IEnumerable<string> Tags { get; }

        public GetTagsDescriptionsQuery(string projectId, IEnumerable<Guid>? audience = null,
            IEnumerable<string>? tags = null)
        {
            ProjectId = projectId;
            Audience = audience ?? System.Array.Empty<Guid>();
            Tags = tags ?? System.Array.Empty<string>();
        }
    }
}
