using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketNoteIdSerializer : SerializerBase<TicketNoteId>
{
    public override TicketNoteId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new TicketNoteId(context.Reader.ReadBinaryData().ToGuid());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TicketNoteId value)
    {
        var data = new BsonBinaryData(value.Value);
        context.Writer.WriteBinaryData(data);
    }
}