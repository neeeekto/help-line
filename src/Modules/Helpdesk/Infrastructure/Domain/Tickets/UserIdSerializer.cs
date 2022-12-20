using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class UserIdSerializer : SerializerBase<UserId>
{
    public override UserId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new UserId(context.Reader.ReadString());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, UserId value)
    {
        context.Writer.WriteString(value.Value);
    }
}