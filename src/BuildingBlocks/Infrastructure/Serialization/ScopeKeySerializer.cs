using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.BuildingBlocks.Infrastructure.Serialization
{
    public class ScopeKeySerializer : SerializerBase<ProjectId>
    {
        public override ProjectId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new ProjectId(context.Reader.ReadString());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProjectId value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}