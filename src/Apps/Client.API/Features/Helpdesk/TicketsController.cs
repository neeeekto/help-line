using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Middlewares;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [Route("v1/hd/tickets")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public TicketsController(IHelpdeskModule helpdeskModule, IExecutionContextAccessor executionContextAccessor)
        {
            _helpdeskModule = helpdeskModule;
            _executionContextAccessor = executionContextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([ProjectParam] string project, CreateTicketRequest request)
        {
            var command = new CreateTicketCommand(
                project,
                request.Language,
                new OperatorInitiatorDto(_executionContextAccessor.UserId),
                request.Tags,
                request.Channels,
                request.UserMeta,
                request.FromTicket,
                request.Message,
                "HelpLine",
                null);
            var ticketId = await _helpdeskModule.ExecuteCommandAsync(command);
            return Ok(ticketId);
        }


        [HttpPut]
        public async Task<ActionResult> Update([ProjectParam] string project)
        {
            // TODO: Mass ops
            return Ok();
        }


    }
}
