using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Projects.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Queries.GetProjects
{
    public class GetProjectsQuery : QueryBase<IEnumerable<ProjectView>>
    {
    }
}
