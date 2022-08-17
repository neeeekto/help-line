using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemoveProblemAndTheme
{
    public class RemoveProblemAndThemeCommand : CommandBase
    {
        public string Tag { get; }
        public string ProjectId { get; }

        public RemoveProblemAndThemeCommand(string tag, string projectId)
        {
            Tag = tag;
            ProjectId = projectId;
        }


    }
}
