using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.DeleteRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.UpdateRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperator;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetRoles;
using HelpLine.Modules.Helpdesk.Application.Operators.ViewModels;
using HelpLine.Modules.UserAccess.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/hd/operators/roles")]
    [Authorize]
    public class OperatorsRolesController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;
        private readonly IUserAccessModule _userAccessModule;
        private readonly IExecutionContextAccessor _contextAccessor;

        public OperatorsRolesController(IHelpdeskModule helpdeskModule, IUserAccessModule userAccessModule, IExecutionContextAccessor contextAccessor)
        {
            _helpdeskModule = helpdeskModule;
            _userAccessModule = userAccessModule;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<OperatorRole>>> Get()
        {
            var roles = await _helpdeskModule.ExecuteQueryAsync(new GetRolesQuery());
            return Ok(roles);
        }

        [HttpGet]
        [Route("{roleId:guid}")]
        public async Task<ActionResult<OperatorRole>> Get(Guid roleId)
        {
            var role = await _helpdeskModule.ExecuteQueryAsync(new GetRoleQuery(roleId));
            return Ok(role);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Guid>> Create(OperatorRoleData data)
        {
            var id = await _helpdeskModule.ExecuteCommandAsync(new CreateRoleCommand(data));
            return Ok(id);
        }

        [HttpPatch]
        [Route("{roleId:guid}")]
        public async Task<ActionResult> Update(Guid roleId, OperatorRoleData data)
        {
            await _helpdeskModule.ExecuteCommandAsync(new UpdateRoleCommand(roleId, data));
            return Ok();
        }

        [HttpDelete]
        [Route("{roleId:guid}")]
        public async Task<ActionResult> Delete(Guid roleId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new DeleteRoleCommand(roleId));
            return Ok();
        }
    }
}
