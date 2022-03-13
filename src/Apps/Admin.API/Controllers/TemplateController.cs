using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Admin.API.Meta;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Services.TemplateRenderer;
using HelpLine.Services.TemplateRenderer.Application.Commands.Delete;
using HelpLine.Services.TemplateRenderer.Application.Commands.Save;
using HelpLine.Services.TemplateRenderer.Application.Queries.GetComponents;
using HelpLine.Services.TemplateRenderer.Application.Queries.GetContexts;
using HelpLine.Services.TemplateRenderer.Application.Queries.GetTemplates;
using HelpLine.Services.TemplateRenderer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Admin.API.Controllers
{
    [ApiController]
    [Route("v1/template-renderer")]
    [Authorize]
    public class TemplateController : ControllerBase
    {
        private readonly TemplateRendererService _templateRenderer;

        private readonly Dictionary<string, Description> _descriptions = new()
        {
            {"feedback-email", new EmailFeedbackDataDescription()},
            {"outgoing-email", new EmailOutgoingMessageDataDescription()},
        };

        public TemplateController(TemplateRendererService templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }

        [HttpGet]
        [Route("templates")]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            var items = await _templateRenderer.ExecuteAsync(new GetTemplatesQuery());
            return Ok(items);
        }

        [HttpPatch]
        [Route("templates")]
        public async Task<ActionResult> UpdateTemplate(Template data)
        {
            await _templateRenderer.ExecuteAsync(new SaveTemplateCommand(data));
            return Ok();
        }

        [HttpDelete]
        [Route("templates/{id}")]
        public async Task<ActionResult> DeleteTemplate(string id)
        {
            await _templateRenderer.ExecuteAsync(new DeleteTemplatesCommand(id));
            return Ok();
        }

        [HttpGet]
        [Route("contexts")]
        public async Task<ActionResult<IEnumerable<Context>>> GetContexts()
        {
            var items = await _templateRenderer.ExecuteAsync(new GetContextsQuery());
            return Ok(items);
        }

        [HttpPatch]
        [Route("contexts")]
        public async Task<ActionResult> UpdateContext(Context data)
        {
            await _templateRenderer.ExecuteAsync(new SaveContextCommand(data));
            return Ok();
        }

        [HttpDelete]
        [Route("contexts/{id}")]
        public async Task<ActionResult> DeleteContext(string id)
        {
            await _templateRenderer.ExecuteAsync(new DeleteContextsCommand(id));
            return Ok();
        }

        [HttpGet]
        [Route("components")]
        public async Task<ActionResult<IEnumerable<Component>>> GetComponents()
        {
            var items = await _templateRenderer.ExecuteAsync(new GetComponentsQuery());
            return Ok(items);
        }

        [HttpPatch]
        [Route("components")]
        public async Task<ActionResult> UpdateComponent(Component data)
        {
            await _templateRenderer.ExecuteAsync(new SaveComponentCommand(data));
            return Ok();
        }

        [HttpDelete]
        [Route("components/{id}")]
        public async Task<ActionResult> DeleteComponent(string id)
        {
            await _templateRenderer.ExecuteAsync(new DeleteComponentsCommand(id));
            return Ok();
        }

        [HttpGet]
        [Route("data-descriptions")]
        public ActionResult<Dictionary<string, Description>> GetDataDescriptions()
        {
            return Ok(_descriptions);
        }
    }
}
