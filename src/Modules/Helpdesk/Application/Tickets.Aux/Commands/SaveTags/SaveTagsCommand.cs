using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTags
{
    public class SaveTagsCommand : CommandBase
    {
        public IEnumerable<string> Keys { get; }
        public string ProjectId { get; }
        public bool Enabled { get; }

        public SaveTagsCommand(IEnumerable<string> keys, string projectId, bool enabled)
        {
            Keys = keys;
            ProjectId = projectId;
            Enabled = enabled;
        }
    }
}
