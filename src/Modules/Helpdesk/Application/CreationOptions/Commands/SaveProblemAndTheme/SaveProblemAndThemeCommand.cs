using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SaveProblemAndTheme
{
    public class SaveProblemAndThemeCommand : CommandBase
    {
        public string ProjectId { get; }
        public string Tag { get;  }
        public ProblemAndTheme Entity { get; }

        public SaveProblemAndThemeCommand(string projectId, string tag, ProblemAndTheme entity)
        {
            Entity = entity;
            Tag = tag;
            ProjectId = projectId;
        }
    }
}
