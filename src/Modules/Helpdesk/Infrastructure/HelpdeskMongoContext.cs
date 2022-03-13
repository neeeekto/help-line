using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.Modules.Helpdesk.Infrastructure
{
    internal class HelpdeskMongoContext : MongoContext
    {
        public HelpdeskMongoContext(string connectionStr, string dbName, ICollectionNameProvider collectionNameProvider) : base(connectionStr, dbName, collectionNameProvider)
        {
        }
    }
}
