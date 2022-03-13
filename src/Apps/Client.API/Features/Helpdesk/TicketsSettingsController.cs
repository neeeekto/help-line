using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetTicketDelayConfiguration;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketsDelayConfiguration;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/settings")]
    [Authorize]
    public class TicketsSettingsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketsSettingsController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<TicketsDelayConfiguration>> Get([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(
                new GetTicketsDelayConfigurationQuery(project)));
        }

        [HttpPatch]
        [Route("")]
        public async Task<ActionResult> Update([ProjectParam] string project,
            [FromBody] TicketDelayConfigurationRequest request)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new SetTicketDelayConfigurationCommand(
                    project,
                    new ReadOnlyDictionary<TicketLifeCycleType, TimeSpan>(request.LifeCycleDelay),
                    request.InactivityDelay,
                    request.FeedbackCompleteDelay
                ));
            return Ok();
        }
    }
}
