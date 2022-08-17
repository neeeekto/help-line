using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.BuildingBlocks.Infrastructure.Serialization
{
    public class PermissionKeySerializer : SerializerBase<PermissionKey>
    {
        public override PermissionKey Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return  new PermissionKey(context.Reader.ReadString());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, PermissionKey value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}