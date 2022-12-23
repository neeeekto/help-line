using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.Modules.Helpdesk.Application.Contracts;
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
    [Route("v1/hd/tags")]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TagsController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetTagsQuery(project)));
        }

        [HttpPost]
        [Route("{tag}")]
        public async Task<ActionResult> SaveTag([ProjectParam] string project, string tag, [FromBody] bool enabled)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SaveTagCommand(tag, project, enabled));
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> SaveTags([ProjectParam] string project, [FromBody] TagsRequest request)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SaveTagsCommand(request.Tags, project, request.Enabled));
            return Ok();
        }

        [HttpDelete]
        [Route("{tag}")]
        public async Task<ActionResult> RemoveTag([ProjectParam] string project, string tag)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RemoveTagCommand(tag, project));
            return Ok();
        }

        [HttpGet]
        [Route("description")]
        public async Task<ActionResult<IEnumerable<TagsDescription>>> GetTagDescription(
            [ProjectParam] string project, [FromQuery] IEnumerable<Guid>? audience,
            [FromQuery] IEnumerable<string>? tags)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(
                new GetTagsDescriptionsQuery(project, audience, tags)));
        }

        [HttpPost]
        [Route("description/{tag}")]
        public async Task<ActionResult> SaveTagDescription([ProjectParam] string project, string tag,
            [FromBody] TagDescriptionRequest request)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SaveTagsDescriptionCommand(project, tag, request.Enabled,
                request.Issues));
            return Ok();
        }

        [HttpDelete]
        [Route("description/{tag}")]
        public async Task<ActionResult> RemoveTagDescription([ProjectParam] string project, string tag)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RemoveTagsDescriptionCommand(tag, project));
            return Ok();
        }
    }
}
