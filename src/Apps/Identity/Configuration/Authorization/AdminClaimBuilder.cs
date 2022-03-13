using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using HelpLine.Apps.Identity.Configuration.Authentication;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity;
using HelpLine.Modules.UserAccess.Application.Identity.Views;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;

namespace HelpLine.Apps.Identity.Configuration.Authorization
{
    public static class AdminClaimBuilder
    {
        public static IEnumerable<Claim> Make(IEnumerable<string> permissions)
        {
            var result = new List<Claim>();
            result.AddRange(permissions.Select(p => new Claim(CustomClaimTypes.Permission, p)));
            return result;
        }

        public static IEnumerable<Claim> Make( )
        {
            var result = new List<Claim>();
            result.Add(new Claim(CustomClaimTypes.FirstName, "Admin"));
            result.Add(new Claim(CustomClaimTypes.LastName, "User"));
            result.Add(new Claim(CustomClaimTypes.Photo, ""));
            result.Add(new Claim(CustomClaimTypes.Language, "en"));
            result.Add(new Claim(CustomClaimTypes.IsAdmin, "true"));
            return result;
        }

    }
}
