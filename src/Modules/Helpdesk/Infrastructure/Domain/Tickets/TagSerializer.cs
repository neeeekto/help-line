using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TagSerializer : SerializerBase<Tag>
{
    public override Tag Deserialize(BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        return new Tag(context.Reader.ReadString());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
        Tag value)
    {
        context.Writer.WriteString(value.Value);
    }
}