using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.BuildingBlocks.Infrastructure.Serialization
{
    public class LanguageCodeSerializer : SerializerBase<LanguageCode>
    {
        public override LanguageCode Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new LanguageCode(context.Reader.ReadString());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, LanguageCode value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}