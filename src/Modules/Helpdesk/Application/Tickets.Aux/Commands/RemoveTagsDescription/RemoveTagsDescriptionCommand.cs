using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTagsDescription
{
    public class RemoveTagsDescriptionCommand : CommandBase
    {
        public string Tag { get; }
        public string ProjectId { get; }

        public RemoveTagsDescriptionCommand(string tag, string projectId)
        {
            Tag = tag;
            ProjectId = projectId;
        }
    }
}
