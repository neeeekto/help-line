using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetProblemAndTheme
{
    public class GetProblemAndThemeQuery : QueryBase<IEnumerable<ProblemAndThemeRoot>>
    {
        public string ProjectId { get; }
        public bool OnlyEnabled { get; }

        public GetProblemAndThemeQuery(string projectId, bool onlyEnabled = false)
        {
            ProjectId = projectId;
            OnlyEnabled = onlyEnabled;
        }
    }
}
