using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.Modules.UserAccess.Infrastructure
{
    internal class UserAccessMongoContext : MongoContext
    {
        public UserAccessMongoContext(string connectionStr, string dbName, ICollectionNameProvider collectionNameProvider) : base(connectionStr, dbName, collectionNameProvider)
        {
        }
    }
}