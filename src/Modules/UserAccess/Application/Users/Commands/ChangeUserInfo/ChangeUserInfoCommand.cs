using System;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Users.DTO;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.ChangeUserInfo
{
    public class ChangeUserInfoCommand : CommandBase
    {
        public Guid UserId { get; }
        public UserInfoDto UserInfo { get; }

        public ChangeUserInfoCommand(Guid userId, UserInfoDto userInfo)
        {
            UserId = userId;
            UserInfo = userInfo;
        }


    }
}
