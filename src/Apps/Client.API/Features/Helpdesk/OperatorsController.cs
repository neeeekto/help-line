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
        [Route("simple")]
        public async Task<ActionResult<IEnumerable<OperatorResponse>>> GetOperators([ProjectParam] string projectId)
        {
            var operators = await _userAccessModule.ExecuteQueryAsync(new GetUsersQuery(projectId));
            return Ok(operators.Select(x => new OperatorResponse()
            {
                Id = x.Id,
                Email = x.Email,
                Photo = x.Info.Photo,
                FirstName = x.Info.FirstName,
                LastName = x.Info.LastName,
                Active = x.Status == UserStatus.Active,
            }));
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

        [HttpGet]
        [Route("{operatorId:guid}")]
        public async Task<ActionResult<OperatorResponse>> GetOperator(Guid operatorId)
        {
            var oper = await _userAccessModule.ExecuteQueryAsync(new GetUserQuery(operatorId));
            return Ok(new OperatorResponse()
            {
                Id = oper.Id,
                Email = oper.Email,
                Photo = oper.Info.Photo,
                FirstName = oper.Info.FirstName,
                LastName = oper.Info.LastName,
                Active = oper.Status == UserStatus.Active
            });
        }

        [HttpPost]
        [Route("favorite/{ticketId}")]
        public async Task<ActionResult> AddFavorite(string ticketId)
        {
            var myId = _contextAccessor.UserId;
            await _helpdeskModule.ExecuteCommandAsync(new AddFavoriteTicketCommand(myId, ticketId));
            return Ok();
        }

        [HttpDelete]
        [Route("favorite/{ticketId}")]
        public async Task<ActionResult> RemoveFavorite(string ticketId)
        {
            var myId = _contextAccessor.UserId;
            await _helpdeskModule.ExecuteCommandAsync(new RemoveFavoriteTicketCommand(myId, ticketId));
            return Ok();
        }
    }
}
