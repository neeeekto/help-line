using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Admin.API.Controllers.Requests;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.ActivateProject;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.CreateProject;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.DeactivateProject;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;
using HelpLine.Modules.Helpdesk.Application.Projects.Queries.GetProjects;
using HelpLine.Modules.Helpdesk.Application.Projects.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.UpdateProject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Admin.API.Controllers
{
    [ApiController]
    [Route("v1/projects")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdesk;

        public ProjectController(IHelpdeskModule helpdesk)
        {
            _helpdesk = helpdesk;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectView>>> Get()
        {
            var projects = await _helpdesk.ExecuteQueryAsync(new GetProjectsQuery());
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateProjectRequest request)
        {
            var project = await _helpdesk.ExecuteCommandAsync(new CreateProjectCommand(request.ProjectId, request));
            return Ok(project);
        }

        [HttpPatch]
        [Route("{projectId}")]
        public async Task<ActionResult> Update([FromBody] ProjectDataDto data, string projectId)
        {
            await _helpdesk.ExecuteCommandAsync(new UpdateProjectCommand(projectId, data));
            return Ok();
        }

        [HttpPost]
        [Route("{projectId}/deactivate")]
        public async Task<ActionResult> Deactivate(string projectId)
        {
            await _helpdesk.ExecuteCommandAsync(new DeactivateProjectCommand(projectId));
            return Ok();
        }

        [HttpPost]
        [Route("{projectId}/activate")]
        public async Task<ActionResult> Activate(string projectId)
        {
            await _helpdesk.ExecuteCommandAsync(new ActivateProjectCommand(projectId));
            return Ok();
        }

    }
}
