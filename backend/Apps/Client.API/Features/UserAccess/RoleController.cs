using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Authorization;
using HelpLine.Apps.Client.API.Features.UserAccess.Requests;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Roles.Commands;
using HelpLine.Modules.UserAccess.Application.Roles.Commands.CreateRole;
using HelpLine.Modules.UserAccess.Application.Roles.Commands.RemoveRole;
using HelpLine.Modules.UserAccess.Application.Roles.Commands.UpdateRole;
using HelpLine.Modules.UserAccess.Application.Roles.Queries;
using HelpLine.Modules.UserAccess.Application.Roles.Queries.GetRoles;
using HelpLine.Modules.UserAccess.Application.Roles.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.UserAccess
{

    [Route("v1/user-access/roles")]
    [ApiController]
    [HasPermissions(UserAccessPermissions.Roles)]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public RoleController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [Route("")]
        [HttpGet]
        [HasPermissions(UserAccessPermissions.ViewRole)]
        public async Task<ActionResult<IEnumerable<RoleView>>> Get()
        {
            var roles = await _userAccessModule.ExecuteQueryAsync(new GetRolesQuery());
            return Ok(roles);
        }

        [Route("")]
        [HttpPost]
        [HasPermissions(UserAccessPermissions.CreateRole)]
        public async Task<ActionResult<Guid>> Create(RoleRequest request)
        {
            return Ok(await _userAccessModule.ExecuteCommandAsync(new CreateRoleCommand(request.Name, request.Permissions)));
        }

        [Route("{roleId:guid}")]
        [HttpPatch]
        [HasPermissions(UserAccessPermissions.UpdateRole)]
        public async Task<ActionResult> Update([FromRoute] Guid roleId, [FromBody] RoleRequest request)
        {
            await _userAccessModule.ExecuteCommandAsync(new UpdateRoleCommand(roleId, request.Name, request.Permissions));
            return Ok();
        }

        [Route("{roleId:guid}")]
        [HttpDelete]
        [HasPermissions(UserAccessPermissions.DeleteRole)]
        public async Task<ActionResult> Delete([FromRoute] Guid roleId)
        {
            await _userAccessModule.ExecuteCommandAsync(new RemoveRoleCommand(roleId));
            return Ok();
        }
    }
}
