using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Features.Helpdesk.Utils;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [Route("v1/hd/reflection")]
    [ApiController]
    [Authorize]
    public class TicketReflectionController : ControllerBase
    {
        private readonly TicketDescription _ticketDescription = new ();
        private readonly TicketFilterCtxDescription _ticketFilterCtxDescription = new ();

        [HttpGet]
        [Route("search-model")]
        public async Task<ActionResult<Description>> GetTicketDescription()
        {
            return Ok(_ticketDescription);
        }

        [HttpGet]
        [Route("ctx-model")]
        public async Task<ActionResult<Description>> GetTicketFilterCtxDescription()
        {
            return Ok(_ticketFilterCtxDescription);
        }
    }
}
