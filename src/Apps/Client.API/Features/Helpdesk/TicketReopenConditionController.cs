using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SwitchTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ToggleTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReopenConditions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/reopen-conditions")]
    [Authorize]
    public class TicketReopenConditionController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketReopenConditionController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<TicketReopenCondition>>> Get([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(
                new GetTicketReopenConditionsQuery(project)));
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save([ProjectParam] string project,
            [FromBody] TicketReopenConditionData request)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new SaveTicketReopenConditionCommand(project, request));
            return Ok();
        }

        [HttpPost]
        [Route("{fromConditionId}/switch")]
        public async Task<ActionResult> Switch([FromBody] string toConditionId, string fromConditionId)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new SwitchTicketReopenConditionCommand(fromConditionId, toConditionId));
            return Ok();
        }

        [HttpPatch]
        [Route("{conditionId}/toggle")]
        public async Task<ActionResult> Toggle([FromBody] bool enable, string conditionId)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new ToggleTicketReopenConditionCommand(conditionId, enable));
            return Ok();
        }

        [HttpDelete]
        [Route("{conditionId}")]
        public async Task<ActionResult> Delete(string conditionId)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new RemoveTicketReopenConditionCommand(conditionId));
            return Ok();
        }
    }
}
