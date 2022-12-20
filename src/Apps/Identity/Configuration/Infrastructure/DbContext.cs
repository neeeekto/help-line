using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.Apps.Identity.Configuration.Infrastructure
{
    public class DbContext : MongoContext
    {
        public DbContext(string connectionStr, string dbName) : base(connectionStr, dbName, new NameProvider())
        {
        }
    }
}
