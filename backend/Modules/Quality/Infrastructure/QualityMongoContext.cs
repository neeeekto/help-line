using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.Modules.Quality.Infrastructure
{
    internal class QualityMongoContext : MongoContext
    {
        public QualityMongoContext(string connectionStr, string dbName, ICollectionNameProvider collectionNameProvider) : base(connectionStr, dbName, collectionNameProvider)
        {
        }
    }
}
