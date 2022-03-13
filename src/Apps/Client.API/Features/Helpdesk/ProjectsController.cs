using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Projects.Queries;
using HelpLine.Modules.Helpdesk.Application.Projects.Queries.GetProjects;
using HelpLine.Modules.Helpdesk.Application.Projects.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [Route("v1/hd/projects")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public ProjectsController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectView>>> Get()
        {
            var items = await _helpdeskModule.ExecuteQueryAsync(new GetProjectsQuery());
            return Ok(items);
        }
    }
}
