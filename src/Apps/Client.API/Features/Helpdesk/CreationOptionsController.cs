using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemovePlatform;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemoveProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SaveProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetPlatforms;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTag;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTagsDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTags;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTagsDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTags;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTagsDescriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/creation-options")]
    [Authorize]
    public class CreationOptionsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public CreationOptionsController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("platforms")]
        public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetPlatformsQuery(project)));
        }

        [HttpDelete]
        [Route("platforms/{key}")]
        public async Task<ActionResult> RemovePlatform([ProjectParam] string project, string key)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RemovePlatformCommand(key, project));
            return Ok();
        }

        [HttpPut]
        [Route("platforms/{key}")]
        public async Task<ActionResult> SavePlatform([ProjectParam] string project, string key,
            [FromBody] PlatformRequest request)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SavePlatformCommand(key, project, request.Name, request.Icon));
            return Ok();
        }

        [HttpGet]
        [Route("problems-and-themes")]
        public async Task<ActionResult<IEnumerable<ProblemAndThemeRoot>>> GetProblemAndThemes(
            [ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetProblemAndThemeQuery(project)));
        }

        [HttpDelete]
        [Route("problems-and-themes/{tag}")]
        public async Task<ActionResult> RemoveProblemAndThemes([ProjectParam] string project, string tag)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RemoveProblemAndThemeCommand(tag, project));
            return Ok();
        }

        [HttpPut]
        [Route("problems-and-themes/{tag}")]
        public async Task<ActionResult> SaveProblemAndThemes([ProjectParam] string project,
            [FromBody] ProblemAndTheme data, string tag)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SaveProblemAndThemeCommand(project, tag, data));
            return Ok();
        }





    }
}
