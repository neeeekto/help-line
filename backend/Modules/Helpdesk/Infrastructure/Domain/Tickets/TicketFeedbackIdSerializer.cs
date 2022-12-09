using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketFeedbackIdSerializer : SerializerBase<TicketFeedbackId>
{
    public override TicketFeedbackId Deserialize(BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        return new TicketFeedbackId(context.Reader.ReadBinaryData().ToGuid());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
        TicketFeedbackId value)
    {
        var data = new BsonBinaryData(value.Value);
        context.Writer.WriteBinaryData(data);
    }
}