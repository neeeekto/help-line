using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketOutgoingMessageIdSerializer : SerializerBase<TicketOutgoingMessageId>
{
    public override TicketOutgoingMessageId Deserialize(BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        return new TicketOutgoingMessageId(context.Reader.ReadBinaryData().ToGuid());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
        TicketOutgoingMessageId value)
    {
        var data = new BsonBinaryData(value.Value);
        context.Writer.WriteBinaryData(data);
    }
}