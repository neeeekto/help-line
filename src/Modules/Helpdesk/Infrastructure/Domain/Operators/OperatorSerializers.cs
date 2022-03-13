using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.Modules.Helpdesk.Domain.Operators;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Operators
{
    internal class OperatorIdSerializer : SerializerBase<OperatorId>
    {
        public override OperatorId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new OperatorId(context.Reader.ReadBinaryData().ToGuid());

        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, OperatorId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }
}
