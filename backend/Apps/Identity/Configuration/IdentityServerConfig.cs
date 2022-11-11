using System.Collections.Generic;
using IdentityServer4.Models;

namespace HelpLine.Apps.Identity.Configuration
{
    public static class IdentityServerConfig
    {

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
