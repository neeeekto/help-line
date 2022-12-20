using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketCommandMap : BsonClassMap<TicketCommandBase>
    {
        public TicketCommandMap()
        {
            SetIsRootClass(true);
            AutoMap();
        }
    }

    internal class AddOutgoingMessageTicketCommandMap : BsonClassMap<AddOutgoingMessageTicketCommand> {
     public AddOutgoingMessageTicketCommandMap() {
       SetDiscriminator(nameof(AddOutgoingMessageTicketCommand));
       AutoMap();
     }
    }
    internal class AddPrivateNoteTicketCommandMap : BsonClassMap<AddPrivateNoteTicketCommand> {
     public AddPrivateNoteTicketCommandMap() {
       SetDiscriminator(nameof(AddPrivateNoteTicketCommand));
       AutoMap();
     }
    }
    internal class AddReminderTicketCommandMap : BsonClassMap<AddReminderTicketCommand> {
     public AddReminderTicketCommandMap() {
       SetDiscriminator(nameof(AddReminderTicketCommand));
       AutoMap();
     }
    }
    internal class AddUserIdTicketCommandMap : BsonClassMap<AddUserIdTicketCommand> {
     public AddUserIdTicketCommandMap() {
       SetDiscriminator(nameof(AddUserIdTicketCommand));
       AutoMap();
     }
    }
    internal class ApproveRejectTicketCommandMap : BsonClassMap<ApproveRejectTicketCommand> {
     public ApproveRejectTicketCommandMap() {
       SetDiscriminator(nameof(ApproveRejectTicketCommand));
       AutoMap();
     }
    }
    internal class AssignTicketCommandMap : BsonClassMap<AssignTicketCommand> {
     public AssignTicketCommandMap() {
       SetDiscriminator(nameof(AssignTicketCommand));
       AutoMap();
     }
    }
    internal class CancelRejectTicketCommandMap : BsonClassMap<CancelRejectTicketCommand> {
     public CancelRejectTicketCommandMap() {
       SetDiscriminator(nameof(CancelRejectTicketCommand));
       AutoMap();
     }
    }
    internal class CancelReminderTicketCommandMap : BsonClassMap<CancelReminderTicketCommand> {
     public CancelReminderTicketCommandMap() {
       SetDiscriminator(nameof(CancelReminderTicketCommand));
       AutoMap();
     }
    }
    internal class ChangeLanguageTicketCommandMap : BsonClassMap<ChangeLanguageTicketCommand> {
     public ChangeLanguageTicketCommandMap() {
       SetDiscriminator(nameof(ChangeLanguageTicketCommand));
       AutoMap();
     }
    }
    internal class ChangePriorityTicketCommandMap : BsonClassMap<ChangePriorityTicketCommand> {
     public ChangePriorityTicketCommandMap() {
       SetDiscriminator(nameof(ChangePriorityTicketCommand));
       AutoMap();
     }
    }
    internal class SetTagsTicketCommandMap : BsonClassMap<SetTagsTicketCommand> {
     public SetTagsTicketCommandMap() {
       SetDiscriminator(nameof(SetTagsTicketCommand));
       AutoMap();
     }
    }
    internal class ChangePrivateNoteTicketCommandMap : BsonClassMap<ChangePrivateNoteTicketCommand> {
     public ChangePrivateNoteTicketCommandMap() {
       SetDiscriminator(nameof(ChangePrivateNoteTicketCommand));
       AutoMap();
     }
    }
    internal class ChangeUserMetaTicketCommandMap : BsonClassMap<ChangeUserMetaTicketCommand> {
     public ChangeUserMetaTicketCommandMap() {
       SetDiscriminator(nameof(ChangeUserMetaTicketCommand));
       AutoMap();
     }
    }
    internal class DenyRejectTicketCommandMap : BsonClassMap<DenyRejectTicketCommand> {
     public DenyRejectTicketCommandMap() {
       SetDiscriminator(nameof(DenyRejectTicketCommand));
       AutoMap();
     }
    }
    internal class ImmediateSendFeedbackTicketCommandMap : BsonClassMap<ImmediateSendFeedbackTicketCommand> {
     public ImmediateSendFeedbackTicketCommandMap() {
       SetDiscriminator(nameof(ImmediateSendFeedbackTicketCommand));
       AutoMap();
     }
    }
    internal class RejectTicketCommandMap : BsonClassMap<RejectTicketCommand> {
     public RejectTicketCommandMap() {
       SetDiscriminator(nameof(RejectTicketCommand));
       AutoMap();
     }
    }
    internal class RemovePrivateNoteTicketCommandMap : BsonClassMap<RemovePrivateNoteTicketCommand> {
     public RemovePrivateNoteTicketCommandMap() {
       SetDiscriminator(nameof(RemovePrivateNoteTicketCommand));
       AutoMap();
     }
    }
    internal class RemoveUserIdTicketCommandMap : BsonClassMap<RemoveUserIdTicketCommand> {
     public RemoveUserIdTicketCommandMap() {
       SetDiscriminator(nameof(RemoveUserIdTicketCommand));
       AutoMap();
     }
    }
    internal class ReopenTicketCommandMap : BsonClassMap<ReopenTicketCommand> {
     public ReopenTicketCommandMap() {
       SetDiscriminator(nameof(ReopenTicketCommand));
       AutoMap();
     }
    }
    internal class ResolveTicketCommandMap : BsonClassMap<ResolveTicketCommand> {
     public ResolveTicketCommandMap() {
       SetDiscriminator(nameof(ResolveTicketCommand));
       AutoMap();
     }
    }
    internal class ChangeHardAssigmentTicketCommandMap : BsonClassMap<ChangeHardAssigmentTicketCommand> {
     public ChangeHardAssigmentTicketCommandMap() {
       SetDiscriminator(nameof(ChangeHardAssigmentTicketCommand));
       AutoMap();
     }
    }
    internal class TogglePendingTicketCommandMap : BsonClassMap<TogglePendingTicketCommand> {
     public TogglePendingTicketCommandMap() {
       SetDiscriminator(nameof(TogglePendingTicketCommand));
       AutoMap();
     }
    }
    internal class ToggleUserChannelTicketCommandMap : BsonClassMap<ToggleUserChannelTicketCommand> {
     public ToggleUserChannelTicketCommandMap() {
       SetDiscriminator(nameof(ToggleUserChannelTicketCommand));
       AutoMap();
     }
    }
    internal class UnassignTicketCommandMap : BsonClassMap<UnassignTicketCommand> {
     public UnassignTicketCommandMap() {
       SetDiscriminator(nameof(UnassignTicketCommand));
       AutoMap();
     }
    }
}
