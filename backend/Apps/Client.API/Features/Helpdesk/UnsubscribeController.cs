using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveFromUnsubscribed;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetUnsubscribes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/unsubscribe")]
    [Authorize]
    public class UnsubscribeController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public UnsubscribeController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Unsubscribe>>> Get([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(
                new GetUnsubscribesQuery(project)));
        }

        [HttpDelete]
        [Route("{unsubscribeId:guid}")]
        public async Task<ActionResult> Delete(Guid unsubscribeId)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new RemoveFromUnsubscribedCommand(unsubscribeId));
            return Ok();
        }

    }
}
