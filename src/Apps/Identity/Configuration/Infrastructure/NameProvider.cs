using HelpLine.BuildingBlocks.Infrastructure.Data;
using IdentityServer4.Models;

namespace HelpLine.Apps.Identity.Configuration.Infrastructure
{
    public class NameProvider : CollectionNameProvider
    {
        public NameProvider() : base("Identity")
        {
            Add<PersistedGrant>("Grants");
        }
    }
}
