using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketsDelayConfiguration
{
    public class GetTicketsDelayConfigurationQuery : QueryBase<TicketsDelayConfiguration>
    {
        public string ProjectId { get; }

        public GetTicketsDelayConfigurationQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
