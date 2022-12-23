using System;
using AutoMapper;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Application.Roles.ViewsModels;
using HelpLine.Modules.UserAccess.Domain.Roles;

namespace HelpLine.Modules.UserAccess.Infrastructure.Application.Roles
{
    internal class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleId, Guid>().ConstructUsing(src => src.Value);
            CreateMap<PermissionKey, string>().ConstructUsing(src => src.Value);
            CreateMap<Role, RoleView>();
        }
    }
}
