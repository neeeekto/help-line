using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Authorization;
using HelpLine.Apps.Client.API.Features.System.Models;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.System
{
    [ApiController]
    [Route("v1/system/state")]
    [Authorize]
    public class StateController : ControllerBase
    {
        private readonly IStorage<AppState> _storage;
        private static string AppStateKey = "Client.API.State";


        public StateController(IStorage<AppState> storage)
        {
            _storage = storage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<AppState>> Get()
        {
            return await _storage.Get(AppStateKey);
        }

        [HttpPost]
        [HasPermissions(SystemPermissions.System)]
        public async Task<ActionResult> Set(AppState state)
        {
            await _storage.Set(AppStateKey, state);

            return Ok();
        }
    }
}
