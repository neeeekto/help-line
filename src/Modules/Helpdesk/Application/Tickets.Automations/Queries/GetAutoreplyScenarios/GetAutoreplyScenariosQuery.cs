using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Queries.GetAutoreplyScenarios
{
    public class GetAutoreplyScenariosQuery : QueryBase<IEnumerable<AutoreplyScenario>>
    {
        public string ProjectId { get; }

        public GetAutoreplyScenariosQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
