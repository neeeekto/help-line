using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag
{
    // Restriction by duplicate - use fronted! I would like that backend will be simple!
    public class SaveTagCommand : CommandBase
    {
        public string Key { get; }
        public string ProjectId { get; }
        public bool Enabled { get; }

        public SaveTagCommand(string key, string projectId, bool enabled)
        {
            Key = key;
            ProjectId = projectId;
            Enabled = enabled;
        }
    }
}
