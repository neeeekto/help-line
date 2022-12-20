using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.RemoveAutoreplyScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.SaveAutoreplyScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/automations")]
    [Authorize]
    public class TicketAutomationsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketAutomationsController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpPost]
        [Route("replies/{scenarioId}")]
        public async Task<ActionResult<string>> SaveAutoreply([ProjectParam] string project, [FromBody] AutoreplyScenario request,
            string scenarioId)
        {
            request.ProjectId = project;
            return Ok(await _helpdeskModule.ExecuteCommandAsync(new SaveAutoreplyScenarioCommand(request)));
        }

        [HttpDelete]
        [Route("replies/{scenarioId}")]
        public async Task<ActionResult> RemoveAutoreply([ProjectParam] string project, string scenarioId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RemoveAutoreplyScenarioCommand(scenarioId));
            return Ok();
        }
    }
}
