using IdentityServer4.Models;

namespace HelpLine.Apps.Identity.Configuration.Authorization
{
    public class HLPersistedGrant : PersistedGrant
    {
        public AuthorizationCode Code { get; set; }
    }
}
