using HelpLine.Modules.Helpdesk.Domain.Interns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class InternHandlingVersionIdSerializer : SerializerBase<InternHandlingVersionId>
{
    public override InternHandlingVersionId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new (context.Reader.ReadBinaryData().ToGuid());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, InternHandlingVersionId value)
    {
        var data = new BsonBinaryData(value.Value);
        context.Writer.WriteBinaryData(data);
    }
}