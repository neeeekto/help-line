using HelpLine.BuildingBlocks.Infrastructure.Data;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    [SpecificMongoSerializer]
    internal class JsonMongoSerializer : SerializerBase<object>
    {
        public override object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bson = context.Reader.ReadString();
            return JsonConvert.DeserializeObject(bson);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {

            var json = JsonConvert.SerializeObject(value);
            context.Writer.WriteString(json);
        }
    }
}
