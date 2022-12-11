using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Response;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.AddFavoriteTicket;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.RemoveFavoriteTicket;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.SetOperatorRoles;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperator;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperators;
using HelpLine.Modules.Helpdesk.Application.Operators.ViewModels;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers;
using HelpLine.Modules.UserAccess.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/operators")]
    [Authorize]
    public class OperatorsController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;
        private readonly IUserAccessModule _userAccessModule;
        private readonly IExecutionContextAccessor _contextAccessor;

        public OperatorsController(IHelpdeskModule helpdeskModule, IExecutionContextAccessor contextAccessor, IUserAccessModule userAccessModule)
        {
            _helpdeskModule = helpdeskModule;
            _contextAccessor = contextAccessor;
            _userAccessModule = userAccessModule;
        }

        [HttpGet]
        [Route("me")]
        public async Task<ActionResult<IEnumerable<OperatorView>>> GetMe()
        {
            var myId = _contextAccessor.UserId;
            var me = await _helpdeskModule.ExecuteQueryAsync(new GetOperatorQuery(myId));
            return Ok(me);
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<OperatorView>>> GetOperators()
        {
            var operators = await _helpdeskModule.ExecuteQueryAsync(new GetOperatorsQuery());
            return Ok(operators);
        }

        [HttpPatch]
        [Route("{operatorId:guid}/roles")]
        public async Task<ActionResult> SetRole([ProjectParam] string project, Guid operatorId, IEnumerable<Guid> rolesIds)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SetOperatorRolesCommand(operatorId, project, rolesIds));
            return Ok();
        }

        [HttpPost]
        [Route("{operatorId:guid}/favorite/{ticketId}")]
        public async Task<ActionResult> AddFavorite(Guid operatorId, string ticketId)
        {
            var myId = _contextAccessor.UserId;
            await _helpdeskModule.ExecuteCommandAsync(new AddFavoriteTicketCommand(myId, ticketId));
            return Ok();
        }

        [HttpDelete]
        [Route("{operatorId:guid}/favorite/{ticketId}")]
        public async Task<ActionResult> RemoveFavorite(Guid operatorId, string ticketId)
        {
            var myId = _contextAccessor.UserId;
            await _helpdeskModule.ExecuteCommandAsync(new RemoveFavoriteTicketCommand(myId, ticketId));
            return Ok();
        }
    }
}
