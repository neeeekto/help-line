using HelpLine.BuildingBlocks.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace HelpLine.Services.Jobs.Infrastructure
{
    [SpecificMongoSerializer]
    internal class JsonMongoSerializer : SerializerBase<object>
    {
        public override object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bson = context.Reader.ReadString();
            return bson == null ? null : JsonConvert.DeserializeObject(bson);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
                return;
            var json = JsonConvert.SerializeObject(value);
            context.Writer.WriteString(json);
        }
    }
}
