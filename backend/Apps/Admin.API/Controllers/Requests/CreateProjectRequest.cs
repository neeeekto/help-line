using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;

namespace HelpLine.Apps.Admin.API.Controllers.Requests
{
    public class CreateProjectRequest : ProjectDataDto
    {
        public string ProjectId { get; set; }
    }
}
