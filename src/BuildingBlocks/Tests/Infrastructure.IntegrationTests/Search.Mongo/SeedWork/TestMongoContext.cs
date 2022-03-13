using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;
using MongoDB.Bson.Serialization;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Mongo.SeedWork
{
    public class TestMongoContext : MongoContext
    {
        static TestMongoContext()
        {
            BsonClassMap.RegisterClassMap(new TestModelMapper());
            BsonClassMap.RegisterClassMap(new TestModelExtendMapper());
            BsonClassMap.RegisterClassMap(new TestModelItemMapper());
            BsonClassMap.RegisterClassMap(new TestModelItemChild1Mapper());
            BsonClassMap.RegisterClassMap(new TestModelItemChild2Mapper());
        }

        public TestMongoContext(string connectionStr, string dbName, ICollectionNameProvider nameProvider) : base(
            connectionStr, dbName, nameProvider)
        {
        }
    }

    public class TestNameProvider : CollectionNameProvider
    {
        public TestNameProvider(string nameSpace) : base(nameSpace)
        {
            Add<TestModel>("TestModels");
        }
    }
}
