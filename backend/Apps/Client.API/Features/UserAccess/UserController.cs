using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Authorization;
using HelpLine.Apps.Client.API.Features.UserAccess.Requests;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity.Commands;
using HelpLine.Modules.UserAccess.Application.Users.Commands;
using HelpLine.Modules.UserAccess.Application.Users.Commands.ChangeUserInfo;
using HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser;
using HelpLine.Modules.UserAccess.Application.Users.Commands.DeleteUser;
using HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserPassword;
using HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserPermissions;
using HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserProjects;
using HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserRoles;
using HelpLine.Modules.UserAccess.Application.Users.DTO;
using HelpLine.Modules.UserAccess.Application.Users.Queries;
using HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.UserAccess
{
    [Route("v1/users-access/users")]
    [ApiController]
    [HasPermissions(UserAccessPermissions.Users)]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public UserController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [Route("")]
        [HttpGet]
        [HasPermissions(UserAccessPermissions.ViewUser)]
        public async Task<ActionResult<IEnumerable<UserView>>> Get([FromQuery] string? projectId)
        {
            var result = await _userAccessModule.ExecuteQueryAsync(new GetUsersQuery(projectId));
            return Ok(result);
        }



        [Route("{userId:guid}")]
        [HttpGet]
        [HasPermissions(UserAccessPermissions.ViewUser)]
        public async Task<ActionResult<UserView>> Get(Guid userId)
        {
            var result = await _userAccessModule.ExecuteQueryAsync(new GetUserQuery(userId));
            return Ok(result);
        }

        [Route("")]
        [HttpPost]
        [HasPermissions(UserAccessPermissions.CreateUser)]
        public async Task<ActionResult<Guid>> Create([FromBody] UserCreateRequest request)
        {
            var command = new CreateUserCommand(request.Info, request.Email, request.GlobalRoles, request.ProjectRoles,
                request.Permissions, request.Projects);
            var result = await _userAccessModule.ExecuteCommandAsync(command);
            return Ok(result);
        }

        [Route("{userId:guid}/info")]
        [HttpPatch]
        [HasPermissions(UserAccessPermissions.UpdateUser)]
        public async Task<ActionResult> UpdateInfo(Guid userId, [FromBody] UserInfoDto request)
        {
            var command = new ChangeUserInfoCommand(userId, request);
            await _userAccessModule.ExecuteCommandAsync(command);
            return Ok();
        }

        [Route("{userId:guid}/password")]
        [HttpPatch]
        [HasPermissions(UserAccessPermissions.SetUserPassword)]
        public async Task<ActionResult> SetPassword(Guid userId, [FromBody] string password)
        {
            var command = new SetUserPasswordCommand(userId, password);
            await _userAccessModule.ExecuteCommandAsync(command);
            return Ok();
        }

        [Route("{userId:guid}")]
        [HttpDelete]
        [HasPermissions(UserAccessPermissions.DeleteUser)]
        public async Task<ActionResult> Delete(Guid userId)
        {
            var command = new DeleteUserCommand(userId);
            await _userAccessModule.ExecuteCommandAsync(command);
            return Ok();
        }

        [Route("{userId:guid}/permissions")]
        [HttpPatch]
        [HasPermissions(UserAccessPermissions.UpdateUserRolesAndPermissions)]
        public async Task<ActionResult> SetPermissions(Guid userId, IEnumerable<string> permissions)
        {
            var command = new SetUserPermissionsCommand(userId, permissions);
            await _userAccessModule.ExecuteCommandAsync(command);
            return Ok();
        }

        [Route("{userId:guid}/roles")]
        [HttpPatch]
        [HasPermissions(UserAccessPermissions.UpdateUserRolesAndPermissions)]
        public async Task<ActionResult> SetRoles(Guid userId, UserRoleRequest request)
        {
            var command = new SetUserRolesCommand(userId, request.GlobalRoles, request.ProjectRoles);
            await _userAccessModule.ExecuteCommandAsync(command);
            return Ok();
        }

        [Route("{userId}/projects")]
        [HttpPatch]
        [HasPermissions(UserAccessPermissions.SetUserProjects)]
        public async Task<ActionResult> SetProjects(Guid userId, IEnumerable<string> projects)
        {
            var command = new SetUserProjectsCommand(userId, projects);
            await _userAccessModule.ExecuteCommandAsync(command);
            return Ok();
        }
    }
}
