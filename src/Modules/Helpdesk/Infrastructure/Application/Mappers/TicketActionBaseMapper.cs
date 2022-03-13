using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketActionBaseMap : BsonClassMap<TicketActionBase>
    {
        public TicketActionBaseMap()
        {
            SetIsRootClass(true);
            AutoMap();
        }
    }

    internal class AddOutgoingMessageActionMap : BsonClassMap<AddOutgoingMessageAction>
    {
        public AddOutgoingMessageActionMap()
        {
            SetDiscriminator(nameof(AddOutgoingMessageAction));
            AutoMap();
        }
    }

    internal class AddTicketNoteActionMap : BsonClassMap<AddTicketNoteAction>
    {
        public AddTicketNoteActionMap()
        {
            SetDiscriminator(nameof(AddTicketNoteAction));
            AutoMap();
        }
    }

    internal class AddTicketReminderActionMap : BsonClassMap<AddTicketReminderAction>
    {
        public AddTicketReminderActionMap()
        {
            SetDiscriminator(nameof(AddTicketReminderAction));
            AutoMap();
        }
    }

    internal class AddUserIdActionMap : BsonClassMap<AddUserIdAction>
    {
        public AddUserIdActionMap()
        {
            SetDiscriminator(nameof(AddUserIdAction));
            AutoMap();
        }
    }

    internal class ApproveTicketRejectActionMap : BsonClassMap<ApproveTicketRejectAction>
    {
        public ApproveTicketRejectActionMap()
        {
            SetDiscriminator(nameof(ApproveTicketRejectAction));
            AutoMap();
        }
    }

    internal class AssignActionMap : BsonClassMap<AssignAction>
    {
        public AssignActionMap()
        {
            SetDiscriminator(nameof(AssignAction));
            AutoMap();
        }
    }

    internal class CancelTicketRejectActionMap : BsonClassMap<CancelTicketRejectAction>
    {
        public CancelTicketRejectActionMap()
        {
            SetDiscriminator(nameof(CancelTicketRejectAction));
            AutoMap();
        }
    }

    internal class CancelTicketReminderActionMap : BsonClassMap<CancelTicketReminderAction>
    {
        public CancelTicketReminderActionMap()
        {
            SetDiscriminator(nameof(CancelTicketReminderAction));
            AutoMap();
        }
    }

    internal class ChangeLanguageActionMap : BsonClassMap<ChangeLanguageAction>
    {
        public ChangeLanguageActionMap()
        {
            SetDiscriminator(nameof(ChangeLanguageAction));
            AutoMap();
        }
    }

    internal class ChangePriorityActionMap : BsonClassMap<ChangePriorityAction>
    {
        public ChangePriorityActionMap()
        {
            SetDiscriminator(nameof(ChangePriorityAction));
            AutoMap();
        }
    }

    internal class ChangeTagsActionMap : BsonClassMap<ChangeTagsAction>
    {
        public ChangeTagsActionMap()
        {
            SetDiscriminator(nameof(ChangeTagsAction));
            AutoMap();
        }
    }

    internal class ChangeTicketNoteActionMap : BsonClassMap<ChangeTicketNoteAction>
    {
        public ChangeTicketNoteActionMap()
        {
            SetDiscriminator(nameof(ChangeTicketNoteAction));
            AutoMap();
        }
    }

    internal class ChangeUserMetaActionMap : BsonClassMap<ChangeUserMetaAction>
    {
        public ChangeUserMetaActionMap()
        {
            SetDiscriminator(nameof(ChangeUserMetaAction));
            AutoMap();
        }
    }

    internal class DenyTicketRejectActionMap : BsonClassMap<DenyTicketRejectAction>
    {
        public DenyTicketRejectActionMap()
        {
            SetDiscriminator(nameof(DenyTicketRejectAction));
            AutoMap();
        }
    }

    internal class ImmediateSendFeedbackActionMap : BsonClassMap<ImmediateSendFeedbackAction>
    {
        public ImmediateSendFeedbackActionMap()
        {
            SetDiscriminator(nameof(ImmediateSendFeedbackAction));
            AutoMap();
        }
    }

    internal class RejectTicketActionMap : BsonClassMap<RejectTicketAction>
    {
        public RejectTicketActionMap()
        {
            SetDiscriminator(nameof(RejectTicketAction));
            AutoMap();
        }
    }

    internal class RemoveTicketNoteActionMap : BsonClassMap<RemoveTicketNoteAction>
    {
        public RemoveTicketNoteActionMap()
        {
            SetDiscriminator(nameof(RemoveTicketNoteAction));
            AutoMap();
        }
    }

    internal class RemoveUserIdActionMap : BsonClassMap<RemoveUserIdAction>
    {
        public RemoveUserIdActionMap()
        {
            SetDiscriminator(nameof(RemoveUserIdAction));
            AutoMap();
        }
    }

    internal class ReopenTicketActionMap : BsonClassMap<ReopenTicketAction>
    {
        public ReopenTicketActionMap()
        {
            SetDiscriminator(nameof(ReopenTicketAction));
            AutoMap();
        }
    }

    internal class ResolveTicketActionMap : BsonClassMap<ResolveTicketAction>
    {
        public ResolveTicketActionMap()
        {
            SetDiscriminator(nameof(ResolveTicketAction));
            AutoMap();
        }
    }

    internal class ToggleHardAssigmentActionMap : BsonClassMap<ToggleHardAssigmentAction>
    {
        public ToggleHardAssigmentActionMap()
        {
            SetDiscriminator(nameof(ToggleHardAssigmentAction));
            AutoMap();
        }
    }

    internal class TogglePendingActionMap : BsonClassMap<TogglePendingAction>
    {
        public TogglePendingActionMap()
        {
            SetDiscriminator(nameof(TogglePendingAction));
            AutoMap();
        }
    }

    internal class ToggleUserChannelActionMap : BsonClassMap<ToggleUserChannelAction>
    {
        public ToggleUserChannelActionMap()
        {
            SetDiscriminator(nameof(ToggleUserChannelAction));
            AutoMap();
        }
    }

    internal class UnassignActionMap : BsonClassMap<UnassignAction>
    {
        public UnassignActionMap()
        {
            SetDiscriminator(nameof(UnassignAction));
            AutoMap();
        }
    }
}
