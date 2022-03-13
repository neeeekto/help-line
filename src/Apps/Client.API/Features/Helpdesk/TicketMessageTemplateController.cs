using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ChangeTicketMessageTemplateOrder;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketMessageTemplate;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketMessageTemplate;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.UpdateTicketMessageTemplate;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketMessageTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/message-templates")]
    [Authorize]
    public class TicketMessageTemplateController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketMessageTemplateController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<TicketMessageTemplate>>> Get([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(
                new GetTicketMessageTemplatesQuery(project)));
        }

        [HttpPost]
        [Route("")]
        public async Task<Guid> Create([ProjectParam] string project, [FromBody] MessageTemplateRequest request)
        {
            return await _helpdeskModule.ExecuteCommandAsync(
                new CreateTicketMessageTemplateCommand(project, request.Contents, request.Group));
        }

        [HttpPatch]
        [Route("{templateId:guid}")]
        public async Task Update(Guid templateId, [ProjectParam] string project,
            [FromBody] MessageTemplateRequest request)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new UpdateTicketMessageTemplateCommand(project, templateId, request.Contents, request.Group));
        }

        [HttpPatch]
        [Route("{templateId:guid}/order")]
        public async Task ChangeOrder(Guid templateId, [FromBody] int newOrder)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new ChangeTicketMessageTemplateOrderCommand(templateId, newOrder));
        }

        [HttpDelete]
        [Route("{templateId:guid}")]
        public async Task Remove(Guid templateId)
        {
            await _helpdeskModule.ExecuteCommandAsync(
                new RemoveTicketMessageTemplateCommand(templateId));
        }
    }
}
