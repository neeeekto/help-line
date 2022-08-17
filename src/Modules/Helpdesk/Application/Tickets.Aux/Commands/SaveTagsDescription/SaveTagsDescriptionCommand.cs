using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTagsDescription
{
    public class SaveTagsDescriptionCommand : CommandBase
    {
        public string ProjectId { get; }
        public string Tag { get; }
        public bool Enabled { get; }
        public IEnumerable<TagsDescriptionIssue> Issues { get; }

        public SaveTagsDescriptionCommand(string projectId, string tag, bool enabled,
            IEnumerable<TagsDescriptionIssue> issues)
        {
            ProjectId = projectId;
            Tag = tag;
            Enabled = enabled;
            Issues = issues;
        }
    }
}
