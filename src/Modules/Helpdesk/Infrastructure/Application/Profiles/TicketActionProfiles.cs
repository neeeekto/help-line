using System.Linq;
using AutoMapper;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Profiles
{
    internal class TicketActionProfiles : Profile
    {
        public TicketActionProfiles()
        {
            CreateMap<AddOutgoingMessageAction, TicketCommandBase>().ConstructUsing((x, ctx) =>
                new AddOutgoingMessageTicketCommand(ctx.Mapper.Map<Message>(x.Message)));
            CreateMap<AddTicketNoteAction, TicketCommandBase>().ConstructUsing((x, ctx) =>
                new AddPrivateNoteTicketCommand(
                    ctx.Mapper.Map<Message>(x.Message), x.Tags));

            CreateMap<AddTicketReminderAction, TicketCommandBase>().ConstructUsing((x, ctx) =>
                new AddReminderTicketCommand(ctx.Mapper.Map<TicketReminder>(x.Reminder)));
            CreateMap<AddUserIdAction, TicketCommandBase>().ConstructUsing((x, ctx) =>
                new AddUserIdTicketCommand(new UserId(x.UserId),
                    new UserChannelState(new Channel(x.Channel), x.UseForDiscussion),
                    x.Main ? UserIdType.Main : UserIdType.Linked));
            CreateMap<ApproveTicketRejectAction, TicketCommandBase>().ConstructUsing((x, ctx) =>
                new ApproveRejectTicketCommand(new TicketAuditId(x.AuditId)));
            CreateMap<AssignAction, TicketCommandBase>()
                .ConstructUsing(x => new AssignTicketCommand(new OperatorId(x.OperatorId)));
            CreateMap<CancelTicketRejectAction, TicketCommandBase>().ConstructUsing(x =>
                new CancelRejectTicketCommand(new TicketAuditId(x.RejectId)));
            CreateMap<CancelTicketReminderAction, TicketCommandBase>().ConstructUsing(x =>
                new CancelReminderTicketCommand(new TicketReminderId(x.ReminderId)));
            CreateMap<ChangeLanguageAction, TicketCommandBase>().ConstructUsing(x =>
                new ChangeLanguageTicketCommand(new LanguageCode(x.Language)));
            CreateMap<ChangePriorityAction, TicketCommandBase>()
                .ConstructUsing(x => new ChangePriorityTicketCommand(x.Priority));
            CreateMap<ChangeTagsAction, TicketCommandBase>().ConstructUsing(x =>
                new SetTagsTicketCommand(x.Tags.Select(x => new Tag(x))));
            CreateMap<ChangeTicketNoteAction, TicketCommandBase>().ConstructUsing((x, ctx) =>
                new ChangePrivateNoteTicketCommand(new TicketNoteId(x.NoteId), ctx.Mapper.Map<Message>(x.Message),
                    x.Tags));
            CreateMap<ChangeUserMetaAction, TicketCommandBase>().ConstructUsing(x =>
                new ChangeUserMetaTicketCommand(new UserMeta(x.Meta)));
            CreateMap<DenyTicketRejectAction, TicketCommandBase>().ConstructUsing(x =>
                new DenyRejectTicketCommand(new TicketAuditId(x.RejectId), new Message(x.Message)));
            CreateMap<ImmediateSendFeedbackAction, TicketCommandBase>().ConstructUsing(x =>
                new ImmediateSendFeedbackTicketCommand());
            CreateMap<RejectTicketAction, TicketCommandBase>().ConstructUsing(x =>
                new RejectTicketCommand(x.Message == null ? null : new Message(x.Message)));
            CreateMap<RemoveTicketNoteAction, TicketCommandBase>().ConstructUsing(x =>
                new RemovePrivateNoteTicketCommand(new TicketNoteId(x.NoteId)));
            CreateMap<RemoveUserIdAction, TicketCommandBase>().ConstructUsing(x =>
                new RemoveUserIdTicketCommand(new UserId(x.UserId)));
            CreateMap<ReopenTicketAction, TicketCommandBase>().ConstructUsing(x => new ReopenTicketCommand());
            CreateMap<ResolveTicketAction, TicketCommandBase>().ConstructUsing(x => new ResolveTicketCommand());
            CreateMap<ToggleHardAssigmentAction, TicketCommandBase>()
                .ConstructUsing(x => new ChangeHardAssigmentTicketCommand(x.HardAssigment));
            CreateMap<TogglePendingAction, TicketCommandBase>()
                .ConstructUsing(x => new TogglePendingTicketCommand(x.Pending));
            CreateMap<ToggleUserChannelAction, TicketCommandBase>().ConstructUsing(x =>
                new ToggleUserChannelTicketCommand(new UserId(x.UserId), x.Enabled));
            CreateMap<UnassignAction, TicketCommandBase>().ConstructUsing(x => new UnassignTicketCommand());
        }
    }
}
