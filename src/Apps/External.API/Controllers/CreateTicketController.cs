using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Apps.External.API.Controllers.Request;
using HelpLine.Apps.External.API.Controllers.Response;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetPlatforms;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.External.API.Controllers
{
    [ApiController]
    [Route("v1/create-ticket/{project}")]
    public class TicketController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpPost]
        [Route("platforms")]
        public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms(string project
            )
        {
            var platforms = await _helpdeskModule.ExecuteQueryAsync(new GetPlatformsQuery(project));
            return Ok(platforms);
        }

        [HttpPost]
        [Route("problem-and-theme")]
        public async Task<ActionResult<IEnumerable<ProblemAndTheme>>> GetProblemAndThemes(string project
        )
        {
            var result = await _helpdeskModule.ExecuteQueryAsync(new GetProblemAndThemeQuery(project, true));
            return Ok(result);
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<TicketCreateResult>> Create(string project,
            [FromBody] NewTicketDataRequest request)
        {
            var userId = request.Channels.FirstOrDefault();
            if (userId == null)
                return BadRequest("Request doesn't have channels");

            var command = new CreateTicketCommand(
                project,
                request.Language,
                new UserInitiatorDto(userId.UserId),
                request.Tags,
                request.Channels.ToDictionary(x => x.UserId, x => x.Channel),
                request.UserMeta.ToDictionary(x => x.Key, x => x.Value),
                null,
                new MessageDto(request.Text, request.Attachments),
                request.Source,
                request.Platform
            );
            try
            {
                var ticketId = await _helpdeskModule.ExecuteCommandAsync(command);
                return new TicketCreateResult(true, ticketId);
            }
            catch (InvalidCommandException e)
            {
                throw;
            }
            catch (BusinessRuleValidationException e)
            {
                throw;
            }
            catch (Exception e)
            {
                // Save to redis;
                return new TicketCreateResult(false);
            }
        }
    }
}
