using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets
{
    internal class TicketStateMongoMap : BsonClassMap<TicketState>
    {
        public TicketStateMongoMap()
        {
            AutoMap();
            MapField("_tags").SetElementName("Tags");
            MapField("_notes").SetElementName("Notes");
            MapField("_outgoingMessages").SetElementName("OutgoingMessages");
            MapField("_lifecycleStatus").SetElementName("LifecycleStatus");
            MapField("_reminders").SetElementName("Reminders");
            MapField("_feedbacks").SetElementName("Feedbacks");
            MapMember(x => x.Priority).SetSerializer(new EnumSerializer<TicketPriority>(BsonType.String));
            MapMember(x => x.Status);
            MapMember(x => x.User);
            MapMember(x => x.AssignedTo).SetIgnoreIfNull(true);
            MapMember(x => x.FallbackStatus);
            MapMember(x => x.HardAssigment);
            MapMember(x => x.HasAnswer);
            MapMember(x => x.ProjectId);
            MapMember(x => x.LastOperatorActivity);
            MapMember(x => x.NeedSendFeedback);
            MapMember(x => x.ActiveApprovalStatusAuditId).SetIgnoreIfNull(true);
        }
    }

    internal class UserMap : BsonClassMap<User>
    {
        public UserMap()
        {
            AutoMap();
            MapField("_ids").SetElementName("Ids");
            MapMember(x => x.Meta);
            UnmapMember(x => x.Channels);
            UnmapMember(x => x.Ids);
        }
    }

    internal class TicketNoteMap : BsonClassMap<TicketNote>
    {
        public TicketNoteMap()
        {
            AutoMap();
            MapMember(x => x.Message);
            MapMember(x => x.Tags);
            MapMember(x => x.NoteId);
        }
    }

    internal class MessageMap : BsonClassMap<Message>
    {
        public MessageMap()
        {
            AutoMap();
            MapMember(x => x.Attachments).SetIgnoreIfNull(true);
            MapMember(x => x.Text);
            MapCreator(x => new Message(x.Text, x.Attachments));
            MapCreator(x => new Message(x.Text));
            MapCreator(x => new Message(x.Attachments));
        }
    }

    internal class InitiatorMap : BsonClassMap<Initiator>
    {
        public InitiatorMap()
        {
            AutoMap();
        }
    }

    internal class OperatorInitiatorMap : BsonClassMap<OperatorInitiator>
    {
        public OperatorInitiatorMap()
        {
            AutoMap();
            SetDiscriminator(nameof(OperatorInitiator));
            MapMember(x => x.OperatorId);
        }
    }

    internal class SystemInitiatorMap : BsonClassMap<SystemInitiator>
    {
        public SystemInitiatorMap()
        {
            AutoMap();
            SetDiscriminator(nameof(SystemInitiator));
            MapMember(x => x.Description);
            MapMember(x => x.Meta);
        }
    }

    internal class UserInitiatorMap : BsonClassMap<UserInitiator>
    {
        public UserInitiatorMap()
        {
            AutoMap();
            SetDiscriminator(nameof(UserInitiator));
            MapMember(x => x.UserId);
        }
    }

    internal class TicketStatusMap : BsonClassMap<TicketStatus>
    {
        public TicketStatusMap()
        {
            AutoMap();
            MapMember(x => x.Kind).SetSerializer(new EnumSerializer<TicketStatusKind>(BsonType.String));
            MapMember(x => x.Type).SetSerializer(new EnumSerializer<TicketStatusType>(BsonType.String));
        }
    }

    internal class TicketReminderMap : BsonClassMap<TicketReminder>
    {
        public TicketReminderMap()
        {
            AutoMap();
            MapMember(x => x.Delay);
            MapMember(x => x.Id);
            MapMember(x => x.Message);
        }
    }

    internal class TicketSequentialReminderMap : BsonClassMap<TicketSequentialReminder>
    {
        public TicketSequentialReminderMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketSequentialReminder));
            MapMember(x => x.Next);
        }
    }

    internal class TicketFinalReminderMap : BsonClassMap<TicketFinalReminder>
    {
        public TicketFinalReminderMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketFinalReminder));
            MapMember(x => x.Resolve);
        }
    }

    internal class TicketFeedbackMap : BsonClassMap<TicketFeedback>
    {
        public TicketFeedbackMap()
        {
            AutoMap();
            MapMember(x => x.Message).SetIgnoreIfNull(true);
            MapMember(x => x.Score);
            MapMember(x => x.Solved).SetIgnoreIfNull(true);
            MapMember(x => x.OptionalScores).SetIgnoreIfNull(true);
            MapCreator(x => new TicketFeedback(x.Score, x.Message, x.Solved, x.OptionalScores));
        }
    }

    internal class TicketReminderStateMap : BsonClassMap<TicketReminderState>
    {
        public TicketReminderStateMap()
        {
            AutoMap();
            MapMember(x => x.Initiator);
            MapMember(x => x.Reminder);
            MapMember(x => x.Status);
            MapMember(x => x.ScheduleId);
        }
    }

    internal class UserIdInfoMap : BsonClassMap<UserIdInfo>
    {
        public UserIdInfoMap()
        {
            AutoMap();
            MapMember(x => x.Channel);
            MapMember(x => x.UseForDiscussion);
            MapMember(x => x.Type);
            MapMember(x => x.UserId);
        }
    }

    internal class UserChannelStateMap : BsonClassMap<UserChannelState>
    {
        public UserChannelStateMap()
        {
            AutoMap();
            MapMember(x => x.Channel);
            MapMember(x => x.Enabled);
        }
    }

    internal class TicketApprovalReplyMap : BsonClassMap<TicketApprovalReply>
    {
        public TicketApprovalReplyMap()
        {
            AutoMap();
            MapMember(x => x.Initiator);
            MapMember(x => x.Message);
            MapMember(x => x.AuditId);
        }
    }

    internal class TicketOutgoingMessageMap : BsonClassMap<TicketOutgoingMessage>
    {
        public TicketOutgoingMessageMap()
        {
            AutoMap();
            MapMember(x => x.Id);
            MapMember(x => x.Initiator);
            MapMember(x => x.Message);
            MapMember(x => x.Statuses);
        }
    }

    internal class TicketMetaMap : BsonClassMap<TicketMeta>
    {
        public TicketMetaMap()
        {
            MapCreator(meta => new TicketMeta(meta.Source, meta.FromTicketId, meta.Platform));
            MapCreator(meta => new TicketMeta(meta.Source));
            AutoMap();
            MapMember(x => x.Source);
            MapMember(x => x.FromTicketId).SetIgnoreIfNull(true);
        }
    }

    internal class UserChannelMap : BsonClassMap<UserChannel>
    {
        public UserChannelMap()
        {
            AutoMap();
            MapMember(x => x.Channel);
            MapMember(x => x.UserId);
        }
    }
}
