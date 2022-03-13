using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketReminder;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketReminder;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.UpdateTicketReminder;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReminders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/reminders")]
    [Authorize]
    public class TicketReminderController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketReminderController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<TicketReminderEntity>>> Get([ProjectParam] string project,
            bool? enabled = null
        )
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(
                new GetTicketRemindersQuery(project, enabled)));
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Guid>> Create([ProjectParam] string project,
            [FromBody] TicketReminderData request)
        {
            return await _helpdeskModule.ExecuteCommandAsync(
                new CreateTicketReminderCommand(project, request));
        }

        [HttpPatch]
        [Route("{reminderId:guid}")]
        public async Task<ActionResult> Update(Guid reminderId, [FromBody] TicketReminderData request)
        {
             await _helpdeskModule.ExecuteCommandAsync(
                new UpdateTicketReminderCommand(reminderId, request));
             return Ok();
        }

        [HttpDelete]
        [Route("{reminderId:guid}")]
        public async Task<ActionResult> Remove(Guid reminderId)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new RemoveTicketReminderCommand(reminderId));
            return Ok();
        }
    }
}
