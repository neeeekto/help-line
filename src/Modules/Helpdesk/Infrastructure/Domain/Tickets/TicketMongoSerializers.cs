using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Interns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
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

    internal class UserMetaSerializer : ReadOnlyDictionaryInterfaceImplementerSerializer<UserMeta, string, string>
    {

    }

    internal class UserChannelsSerializer : ReadOnlyCollectionSerializer<UserChannels>
    {

    }

    internal class ScheduleIdSerializer : SerializerBase<ScheduleId>
    {
        public override ScheduleId Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return new ScheduleId(context.Reader.ReadBinaryData().ToGuid());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            ScheduleId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }

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

    internal class TicketAuditIdSerializer : SerializerBase<TicketAuditId>
    {
        public override TicketAuditId Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return new TicketAuditId(context.Reader.ReadBinaryData().ToGuid());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            TicketAuditId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }

    internal class TagSerializer : SerializerBase<Tag>
    {
        public override Tag Deserialize(BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return new Tag(context.Reader.ReadString());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args,
            Tag value)
        {
            context.Writer.WriteString(value.Value);
        }
    }

    internal class InternTicketIdSerializer : SerializerBase<InternTicketId>
    {
        public override InternTicketId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new (context.Reader.ReadBinaryData().ToGuid());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, InternTicketId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }

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
}
