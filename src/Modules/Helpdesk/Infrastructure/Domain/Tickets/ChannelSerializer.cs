using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class ChannelSerializer : SerializerBase<Channel>
{
    public override Channel Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new Channel(context.Reader.ReadString());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Channel value)
    {
        context.Writer.WriteString(value.Value);
    }
}