using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Apps.Admin.API.Controllers.Requests;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.DeleteScheduleTimer;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ReScheduleTimer;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetSchedules;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.SaveChannelSetting;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetChannelSettings;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SyncTicketView;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Admin.API.Controllers
{
    [ApiController]
    [Route("v1/helpdesk")]
    [Authorize]
    public class HelpdeskController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public HelpdeskController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("schedules")]
        public async Task<ActionResult<IEnumerable<TicketSchedule>>> GetSchedules(
            [FromQuery] IEnumerable<TicketSchedule.Statuses> statuses)
        {
            var result = await _helpdeskModule.ExecuteQueryAsync(new GetSchedulesQuery(statuses));
            return Ok(result);
        }

        [HttpGet]
        [Route("ticket/{ticketId}/schedules")]
        public async Task<ActionResult<IEnumerable<TicketSchedule>>> GetSchedulesByTicket(string ticketId)
        {
            var result = await _helpdeskModule.ExecuteQueryAsync(new GetSchedulesByTicketQuery(ticketId));
            return Ok(result);
        }

        [HttpPost]
        [Route("schedules/{scheduleId:guid}/reschedule")]
        public async Task<ActionResult> ReScheduleTimer(Guid scheduleId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new ReScheduleTimerCommand(scheduleId));
            return Ok();
        }

        [HttpDelete]
        [Route("schedules/{scheduleId:guid}")]
        public async Task<ActionResult> DeleteTimer(Guid scheduleId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new DeleteScheduleTimerCommand(scheduleId));
            return Ok();
        }

        [HttpPost]
        [Route("ticket")]
        public async Task<ActionResult<string>> CreateTicket([FromBody] CreateTicketRequest request)
        {
            var channelItem = request.Channels.FirstOrDefault();
            if (channelItem == null)
                return BadRequest("Request doesn't have channels");

            var ticketId = await _helpdeskModule.ExecuteCommandAsync(new CreateTicketCommand(request.Project,
                request.Language,
                new UserInitiatorDto(channelItem.UserId),
                request.Tags ?? Array.Empty<string>(),
                request.Channels.ToDictionary(x => x.UserId, x => x.Channel),
                request.UserMeta?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, string>(),
                request.FromTicket,
                new MessageDto(request.Text, request.Attachments),
                "HelpLine Admin", request.Platform));
            return Ok(ticketId);
        }

        [HttpGet]
        [Route("ticket/{ticketId}/view")]
        public async Task<ActionResult<TicketView>> GetTicketView(string ticketId)
        {
            var ticket = await _helpdeskModule.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            return Ok(ticket);
        }

        [HttpPost]
        [Route("ticket/{ticketId}/view/recreate")]
        public async Task<ActionResult> RecreateView(string ticketId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SyncTicketViewCommand(ticketId));
            return Ok();
        }

        [HttpGet]
        [Route("channels")]
        public async Task<ActionResult<IEnumerable<ChannelSettings>>> GetChannelsSystems()
        {
            var result = await _helpdeskModule.ExecuteQueryAsync(new GetChannelSettingsQuery());
            return Ok(result);
        }

        [HttpPatch]
        [Route("channels")]
        public async Task<ActionResult> UpdateChannelsSystems(ChannelSettings settings)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SaveChannelSettingCommand(settings));
            return Ok();
        }
    }
}
