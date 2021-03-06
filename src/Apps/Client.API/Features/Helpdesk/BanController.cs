using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.AddBan;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveBan;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBans;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBanSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/ban")]
    [Authorize]
    public class BanController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public BanController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("settings")]
        public async Task<ActionResult<BanSettings>> GetBanSettings([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetBanSettingsQuery(project)));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ban>>> GetBans([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetBansQuery(project)));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddBan([ProjectParam] string project, BanRequest request)
        {
            return Ok(await _helpdeskModule.ExecuteCommandAsync(new AddBanCommand(project, request.Parameter,
                request.Value, request.ExpiredAt)));
        }

        [HttpDelete]
        [Route("{banId:guid}")]
        public async Task<ActionResult> RemoveBan([ProjectParam] string project, Guid banId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RemoveBanCommand(banId));
            return Ok();
        }
    }
}
