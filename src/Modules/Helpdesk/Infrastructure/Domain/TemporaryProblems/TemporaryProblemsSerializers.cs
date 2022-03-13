using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.TemporaryProblems;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.TemporaryProblems
{
    internal class TemporaryProblemIdSerializer : SerializerBase<TemporaryProblemId>
    {
        public override TemporaryProblemId Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return new TemporaryProblemId(context.Reader.ReadBinaryData().ToGuid());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            TemporaryProblemId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }

    internal class TemporaryProblemUpdateIdSerializer : SerializerBase<TemporaryProblemUpdateId>
    {
        public override TemporaryProblemUpdateId Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return new TemporaryProblemUpdateId(context.Reader.ReadBinaryData().ToGuid());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            TemporaryProblemUpdateId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }

    internal class TemporaryProblemDescriptionSerializer : ReadOnlyDictionaryInterfaceImplementerSerializer<
        LocalizeDictionary<TemporaryProblemInfo>, LanguageCode, TemporaryProblemInfo>
    {
    }

    internal class TemporaryProblemSubscriberEmailSerializer : SerializerBase<TemporaryProblemSubscriberEmail>
    {
        public override TemporaryProblemSubscriberEmail Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return new TemporaryProblemSubscriberEmail(context.Reader.ReadString());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            TemporaryProblemSubscriberEmail value)
        {
            context.Writer.WriteString(value.Value);
        }
    }
}
