using System.Collections.Generic;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Mongo.SeedWork
{
    public class TestModelMapper : BsonClassMap<TestModel>
    {
        public TestModelMapper()
        {
            AutoMap();
            SetIsRootClass(true);
            MapField(x => x.Enum).SetSerializer(new EnumSerializer<TestModelEnum>(BsonType.String));
            MapField(x => x.Dictionary)
                .SetSerializer(
                    new DictionaryInterfaceImplementerSerializer<Dictionary<string, string>, string, string>(DictionaryRepresentation.ArrayOfDocuments));
        }
    }

    public class TestModelExtendMapper : BsonClassMap<TestModelExtend>
    {
        public TestModelExtendMapper()
        {
            AutoMap();
            SetDiscriminator(nameof(TestModelExtend));
        }
    }

    public class TestModelItemMapper : BsonClassMap<TestModelItem>
    {
        public TestModelItemMapper()
        {
            AutoMap();
            SetIsRootClass(true);
        }
    }

    public class TestModelItemChild1Mapper : BsonClassMap<TestModelItemChild1>
    {
        public TestModelItemChild1Mapper()
        {
            SetDiscriminator(nameof(TestModelItemChild1));
            AutoMap();
        }
    }

    public class TestModelItemChild2Mapper : BsonClassMap<TestModelItemChild2>
    {
        public TestModelItemChild2Mapper()
        {
            SetDiscriminator(nameof(TestModelItemChild2));
            AutoMap();
        }
    }
}
