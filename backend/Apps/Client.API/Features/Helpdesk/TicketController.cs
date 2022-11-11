using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Correctors;
using HelpLine.Apps.Client.API.Configuration.Middlewares;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RetryOutgoingMessage;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicketAtTime;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [Route("v1/hd/ticket/{ticketId}")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly Corrector<TicketView> _corrector;

        public TicketController(IHelpdeskModule helpdeskModule,
            IExecutionContextAccessor executionContextAccessor, Corrector<TicketView> corrector)
        {
            _helpdeskModule = helpdeskModule;
            _executionContextAccessor = executionContextAccessor;
            _corrector = corrector;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<TicketView>> GetTicket([FromRoute] string ticketId)
        {
            var result = await _helpdeskModule.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            await _corrector.Correct(result);
            return Ok(result);
        }

        [HttpGet]
        [Route("{date:datetime}")]
        public async Task<ActionResult<TicketView>> GetTicketAtDate([FromRoute] string ticketId,
            [FromRoute] DateTime date)
        {
            var result = await _helpdeskModule.ExecuteQueryAsync(new GetTicketAtTimeQuery(ticketId, date));
            await _corrector.Correct(result);
            return Ok(result);
        }

        [HttpPost]
        [Route("messages/{userId}/{messageId:guid}/retry")]
        public async Task<ActionResult> RetryMessage([FromRoute] string ticketId,
            IEnumerable<TicketActionBase> actions, Guid messageId, string userId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RetryOutgoingMessageCommand(ticketId, messageId,
                userId, new OperatorInitiatorDto(_executionContextAccessor.UserId)));
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Execute([FromRoute] string ticketId,
            IEnumerable<TicketActionBase> actions, [VersionParam] int version)
        {
            var currentVersion = await _helpdeskModule.ExecuteQueryAsync(new GetTicketVersionQuery(ticketId));
            if (version != currentVersion)
            {
                return Conflict($"Ticket version not match, current: ${currentVersion}, needed: ${version}");
            }
            var result = await _helpdeskModule.ExecuteCommandAsync(new ExecuteTicketActionsCommand(ticketId, actions,
                new OperatorInitiatorDto(_executionContextAccessor.UserId)));
            return Ok(result);
        }
    }
}
