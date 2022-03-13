using HelpLine.Modules.Helpdesk.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [Route("v1/hd/macros")]
    [ApiController]
    [Authorize]
    public class TicketMacrosController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;

        public TicketMacrosController(IHelpdeskModule helpdeskModule)
        {
            _helpdeskModule = helpdeskModule;
        }
    }
}
