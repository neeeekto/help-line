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
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetBanSetting;
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
    [Route("v1/hd/ban-settings")]
    [Authorize]
    public class BanSettingsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public BanSettingsController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<BanSettings>> GetBanSettings([ProjectParam] string project)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetBanSettingsQuery(project)));
        }
        
        [HttpPut]
        [Route("")]
        public async Task<ActionResult> SetBanSettings([ProjectParam] string project, BanSettings settings)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SetBanSettingCommand(project, settings));
            return Ok();
        }
    }
}
