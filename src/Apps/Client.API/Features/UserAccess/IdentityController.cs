using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.UserAccess
{
    [ApiController]
    [Route("v1/users-access/identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;
        private readonly IExecutionContextAccessor _accessor;

        public IdentityController(IUserAccessModule userAccessModule, IExecutionContextAccessor accessor)
        {
            _userAccessModule = userAccessModule;
            _accessor = accessor;
        }

        [HttpGet]
        public async Task<ActionResult<UserView>> Me()
        {
            if (_accessor.UserId == Guid.Empty)
                return Ok(null);

            var result = await _userAccessModule.ExecuteQueryAsync(new GetUserQuery(_accessor.UserId));
            return Ok(result);
        }
    }
}
