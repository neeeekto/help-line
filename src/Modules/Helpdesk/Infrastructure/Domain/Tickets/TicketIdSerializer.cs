using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MongoDB.Bson.Serialization.Options;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets
{
    internal class TicketIdSerializer : SerializerBase<TicketId>
    {
        public override TicketId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new TicketId(context.Reader.ReadString());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TicketId value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}
