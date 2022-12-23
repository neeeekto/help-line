using HelpLine.Apps.Client.API.Configuration.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.System
{
    [ApiController]
    [Route("v1/system/settings")]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly SystemSettings _settings;

        public SettingsController(SystemSettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<SystemSettings> Get()
        {
            return Ok( _settings);
        }
    }
}
