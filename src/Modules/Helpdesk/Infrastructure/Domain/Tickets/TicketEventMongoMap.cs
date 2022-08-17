using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets
{
    internal class TicketEventMongoMap : BsonClassMap<TicketEventBase>
    {
        public TicketEventMongoMap()
        {
            AutoMap();
            MapMember(x => x.Initiator);
            MapMember(x => x.CreateDate);
        }
    }

    internal class TicketAssignChangedEventMap : BsonClassMap<TicketAssignChangedEvent>
    {
        public TicketAssignChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketAssignChangedEvent));
            MapMember(x => x.Assignee).SetIgnoreIfNull(true);
        }
    }

    internal class TicketAssingmentBindingChangedEventMap : BsonClassMap<TicketAssingmentBindingChangedEvent>
    {
        public TicketAssingmentBindingChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketAssingmentBindingChangedEvent));
            MapMember(x => x.HardAssigment);
        }
    }

    internal class TicketClosedEventMap : BsonClassMap<TicketClosedEvent>
    {
        public TicketClosedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketClosedEvent));
        }
    }

    internal class TicketCreatedEventMap : BsonClassMap<TicketCreatedEvent>
    {
        public TicketCreatedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketCreatedEvent));
            MapMember(x => x.UserChannels);
            MapMember(x => x.Priority).SetSerializer(new EnumSerializer<TicketPriority>(BsonType.String));
            MapMember(x => x.Language);
            MapMember(x => x.Meta);
            MapMember(x => x.Status);
            MapMember(x => x.Tags);
            MapMember(x => x.ProjectId);
            MapMember(x => x.UserMeta);
            MapMember(x => x.Message).SetIgnoreIfNull(true);
        }
    }

    internal class TicketIncomingMessageAddedEventMap : BsonClassMap<TicketIncomingMessageAddedEvent>
    {
        public TicketIncomingMessageAddedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketIncomingMessageAddedEvent));
            MapMember(x => x.Channel);
            MapMember(x => x.Message);
            MapMember(x => x.UserId);
        }
    }

    internal class TicketLanguageChangedEventMap : BsonClassMap<TicketLanguageChangedEvent>
    {
        public TicketLanguageChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketLanguageChangedEvent));
            MapMember(x => x.Language);
        }
    }

    internal class TicketMessageStatusChangedEventMap : BsonClassMap<TicketMessageStatusChangedEvent>
    {
        public TicketMessageStatusChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketMessageStatusChangedEvent));
            MapMember(x => x.Detail).SetIgnoreIfNull(true);
            MapMember(x => x.Status);
            MapMember(x => x.MessageId);
            MapMember(x => x.UserId);
        }
    }

    internal class TicketNoteAddedEventMap : BsonClassMap<TicketNotePostedEvent>
    {
        public TicketNoteAddedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketNotePostedEvent));
            MapMember(x => x.Message);
            MapMember(x => x.Tags);
            MapMember(x => x.NoteId);
        }
    }

    internal class TicketNoteRemovedEventMap : BsonClassMap<TicketNoteRemovedEvent>
    {
        public TicketNoteRemovedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketNoteRemovedEvent));
            MapMember(x => x.NoteId);
        }
    }

    internal class TicketOutgoingMessageAddedEventMap : BsonClassMap<TicketOutgoingMessageAddedEvent>
    {
        public TicketOutgoingMessageAddedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketOutgoingMessageAddedEvent));
            MapMember(x => x.Recipients);
            MapMember(x => x.Message);
            MapMember(x => x.Recipients);
            MapMember(x => x.MessageId);
        }
    }

    internal class TicketPriorityChangedEventMap : BsonClassMap<TicketPriorityChangedEvent>
    {
        public TicketPriorityChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketPriorityChangedEvent));
            MapMember(x => x.Priority).SetSerializer(new EnumSerializer<TicketPriority>(BsonType.String));
            ;
        }
    }

    internal class TicketStatusChangedEventMap : BsonClassMap<TicketStatusChangedEvent>
    {
        public TicketStatusChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketStatusChangedEvent));
            MapMember(x => x.Status);
        }
    }

    internal class TicketTagsChangedEventMap : BsonClassMap<TicketTagsChangedEvent>
    {
        public TicketTagsChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketTagsChangedEvent));
            MapMember(x => x.Tags);
        }
    }

    internal class TicketAutoreplyEventMap : BsonClassMap<TicketAutoreplySendedEvent>
    {
        public TicketAutoreplyEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketAutoreplySendedEvent));
            MapMember(x => x.Message);
            MapMember(x => x.Reminder).SetIgnoreIfNull(true);
            MapMember(x => x.Resolve);
            MapMember(x => x.Tags);
        }
    }

    internal class TicketFeedbackAddedEventMap : BsonClassMap<TicketFeedbackAddedEvent>
    {
        public TicketFeedbackAddedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketFeedbackAddedEvent));
            MapMember(x => x.Feedback);
            MapMember(x => x.FeedbackId);
        }
    }

    internal class TicketFeedbackSentEventMap : BsonClassMap<TicketFeedbackSentEvent>
    {
        public TicketFeedbackSentEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketFeedbackSentEvent));
            MapMember(x => x.FeedbackId);
        }
    }

    internal class TicketReminderCanceledEventMap : BsonClassMap<TicketReminderCanceledEvent>
    {
        public TicketReminderCanceledEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketReminderCanceledEvent));
            MapMember(x => x.ReminderId);
        }
    }

    internal class TicketReminderExecutedEventMap : BsonClassMap<TicketReminderExecutedEvent>
    {
        public TicketReminderExecutedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketReminderExecutedEvent));
            MapMember(x => x.ReminderId);
        }
    }

    internal class TicketReminderScheduledEventMap : BsonClassMap<TicketReminderScheduledEvent>
    {
        public TicketReminderScheduledEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketReminderScheduledEvent));
            MapMember(x => x.Reminder);
            MapMember(x => x.ExecutionDate);
        }
    }

    internal class TicketScheduleEventBaseMap : BsonClassMap<TicketScheduleEventBase>
    {
        public TicketScheduleEventBaseMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketScheduleEventBase));
            MapMember(x => x.ScheduleId);
        }
    }

    internal class TicketTimerPlannedEventMap : BsonClassMap<TicketLifecyclePlannedEvent>
    {
        public TicketTimerPlannedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketLifecyclePlannedEvent));
            MapMember(x => x.ExecutionDate);
            MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));
        }
    }

    internal class TicketTimerCanceledEventMap : BsonClassMap<TicketLifecycleCanceledEvent>
    {
        public TicketTimerCanceledEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketLifecycleCanceledEvent));
            MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));
        }
    }

    internal class TicketTimerProlongatedEventMap : BsonClassMap<TicketLifecycleProlongatedEvent>
    {
        public TicketTimerProlongatedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketLifecycleProlongatedEvent));
            MapMember(x => x.NextDate);
            MapMember(x => x.ScheduleId);
            MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));
        }
    }

    internal class TicketUserIdsChangedEventMap : BsonClassMap<TicketUserIdsChangedEvent>
    {
        public TicketUserIdsChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketUserIdsChangedEvent));
            MapMember(x => x.UserIds);

        }
    }

    internal class TicketApprovalStatusAddedEventMap : BsonClassMap<TicketApprovalStatusAddedEvent>
    {
        public TicketApprovalStatusAddedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketApprovalStatusAddedEvent));
            MapMember(x => x.Message);
            MapMember(x => x.Status);
            MapMember(x => x.AuditId);

        }
    }

    internal class TicketApprovalStatusApprovedEventMap : BsonClassMap<TicketApprovalStatusApprovedEvent>
    {
        public TicketApprovalStatusApprovedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketApprovalStatusApprovedEvent));
            MapMember(x => x.AuditId);

        }
    }

    internal class TicketApprovalStatusCanceledEventMap : BsonClassMap<TicketApprovalStatusCanceledEvent>
    {
        public TicketApprovalStatusCanceledEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketApprovalStatusCanceledEvent));
            MapMember(x => x.AuditId);

        }
    }

    internal class TicketApprovalStatusDeniedEventMap : BsonClassMap<TicketApprovalStatusDeniedEvent>
    {
        public TicketApprovalStatusDeniedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketApprovalStatusDeniedEvent));
            MapMember(x => x.Message);
            MapMember(x => x.AuditId);

        }
    }

    internal class TicketLifecycleExecutedEventMap : BsonClassMap<TicketLifecycleExecutedEvent>
    {
        public TicketLifecycleExecutedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketLifecycleExecutedEvent));
            MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));

        }
    }

    internal class TicketUserMetaChangedEventMap : BsonClassMap<TicketUserMetaChangedEvent>
    {
        public TicketUserMetaChangedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketUserMetaChangedEvent));
            MapMember(x => x.Meta);

        }
    }

    internal class TicketUserUnsubscribedEventMap : BsonClassMap<TicketUserUnsubscribedEvent>
    {
        public TicketUserUnsubscribedEventMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketUserUnsubscribedEvent));
        }
    }
}
