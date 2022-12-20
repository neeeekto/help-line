using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Infrastructure.Application.Users
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserId, Guid>().ConstructUsing(src => src.Value);
            CreateMap<User, UserView>();
            CreateMap<UserInfo, UserInfoView>();
            CreateMap<IEnumerable<ProjectId>, IEnumerable<string>>().ConstructUsing(x => x.Select(x => x.Value));
        }
    }
}
