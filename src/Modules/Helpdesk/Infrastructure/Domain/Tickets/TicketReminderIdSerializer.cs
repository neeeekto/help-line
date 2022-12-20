using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketReminderIdSerializer : SerializerBase<TicketReminderId>
{
    public override TicketReminderId Deserialize(BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        return new TicketReminderId(context.Reader.ReadBinaryData().ToGuid());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
        TicketReminderId value)
    {
        var data = new BsonBinaryData(value.Value);
        context.Writer.WriteBinaryData(data);
    }
}