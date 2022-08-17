using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity;
using HelpLine.Modules.UserAccess.Application.Identity.Views;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;

namespace HelpLine.Apps.Identity.Configuration.Authorization
{
    public static class UserClaimBuilder
    {
        public static IEnumerable<Claim> Make( UserPermissionsView permissions)
        {
            var result = new List<Claim>();
            result.AddRange(permissions.Global.Select(gPermission => new Claim(CustomClaimTypes.Permission, gPermission)));
            result.AddRange(permissions.Custom.Select(gPermission => new Claim(CustomClaimTypes.Permission, gPermission)));
            result.AddRange(from permissionsByProject in permissions.ByProjects
                from perm in permissionsByProject.Value
                select new Claim(CustomClaimTypes.ByProject(permissionsByProject.Key), perm));
            return result;
        }

        public static IEnumerable<Claim> Make( UserView user)
        {
            var result = new List<Claim>();
            result.Add(new Claim(CustomClaimTypes.UserId, user.Id.ToString()));
            result.Add(new Claim(CustomClaimTypes.FirstName, user.Info.FirstName));
            result.Add(new Claim(CustomClaimTypes.LastName, user.Info.LastName));
            result.Add(new Claim(CustomClaimTypes.Photo, user.Info.Photo));
            result.Add(new Claim(CustomClaimTypes.Language, user.Info.Language));
            return result;
        }
    }
}
