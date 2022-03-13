using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBanSettings
{
    public class GetBanSettingsQuery : QueryBase<BanSettings>
    {
        public string ProjectId { get; }

        public GetBanSettingsQuery(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
