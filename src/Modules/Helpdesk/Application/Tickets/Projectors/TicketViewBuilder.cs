using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Extensions;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Projectors
{
    internal class TicketViewBuilder
    {
        public TicketViewBuilder(TicketView? ticket = null)
        {
            Ticket = ticket ?? new TicketView();
        }

        public TicketViewBuilder Project(EventBase<TicketId> evt)
        {
            When((dynamic) evt);
            return this;
        }
        public TicketViewBuilder Project(IEnumerable<EventBase<TicketId>> evts)
        {
            foreach (var evt in evts)
                Project(evt);
            return this;
        }

        public TicketView Ticket { get; private set; }

        private void AddEvent(TicketEventView evt)
        {
            Ticket.Events = Ticket.Events.Concat(evt);
        }

        private static void When(TicketEventBase evt)
        {
        }


        private void When(TicketCreatedEvent evt)
        {
            var ticket = new TicketView
            {
                Id = evt.AggregateId.Value,
                Language = evt.Language.Value,
                Tags = evt.Tags.Select(x => x.Value),
                Title = evt.Message?.Text ?? "",
                CreateDate = evt.CreateDate,
                Priority = evt.Priority,
                Status = new TicketStatusView(evt.Status),
                ProjectId = evt.ProjectId.Value,
                UserMeta = evt.UserMeta.ToDictionary(x => x.Key, x => x.Value),
                HardAssigment = false,
                DateOfLastStatusChange = evt.CreateDate,
                DiscussionState = new TicketDiscussionStateView
                {
                    IterationCount = 0,
                    LastMessageType = TicketDiscussionStateView.MessageType.Incoming,
                    LastReplyDate = null,
                    HasAttachments = false,
                },
                Meta = new TicketMetaView(evt.Meta),
            };
            ticket.UserIds = ticket.UserIds.Concat(evt.UserChannels.Select(x => new UserIdInfoView
                {Channel = x.Channel.Value, UserId = x.UserId.Value, UseForDiscussion = true, Type = UserIdType.Main}));
            Ticket = ticket;

            AddEvent(new TicketCreatedEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                UserIds = ticket.UserIds,
                Language = evt.Language.Value,
                Message = evt.Message != null ? new MessageView(evt.Message) : null,
                Priority = evt.Priority,
                Status = evt.Status,
                Tags = evt.Tags.Select(x => x.Value),
                CreateDate = evt.CreateDate,
                ProjectId = evt.ProjectId.Value,
                UserMeta = evt.UserMeta,
            });
            if (evt.Message?.Attachments?.Count() > 0)
                Ticket.DiscussionState.HasAttachments = true;
        }

        private void When(TicketStatusChangedEvent evt)
        {
            AddEvent(new TicketStatusChangedEventView
            {
                Id = evt.Id, Initiator = InitiatorMapper.Map(evt.Initiator),
                New = new TicketStatusView(evt.Status),
                Old = Ticket.Status,
                CreateDate = evt.CreateDate
            });
            Ticket.Status = new TicketStatusView(evt.Status);
            Ticket.DateOfLastStatusChange = evt.CreateDate;
        }

        private void When(TicketPriorityChangedEvent evt)
        {
            AddEvent(new TicketPriorityEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                New = evt.Priority, Old = Ticket.Priority,
                CreateDate = evt.CreateDate
            });
            Ticket.Priority = evt.Priority;
        }

        private void When(TicketAssignChangedEvent evt)
        {
            AddEvent(new TicketAssigmentEventView()
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                From = Ticket.AssignedTo,
                To = evt.Assignee?.Value,
                CreateDate = evt.CreateDate
            });

            Ticket.AssignedTo = evt.Assignee?.Value;
        }

        private void When(TicketAssingmentBindingChangedEvent evt)
        {
            AddEvent(new TicketAssigmentBindingEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                HardAssigment = evt.HardAssigment,
                CreateDate = evt.CreateDate
            });

            Ticket.HardAssigment = evt.HardAssigment;
        }


        private void When(TicketFeedbackSentEvent evt)
        {
            AddEvent(new TicketFeedbackEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                FeedbackId = evt.FeedbackId.Value,
                CreateDate = evt.CreateDate,
            });
        }

        private void When(TicketFeedbackAddedEvent evt)
        {
            Ticket.Feedbacks = Ticket.Feedbacks.Concat(new[]
            {
                new FeedbackReviewView
                {
                    FeedbackId = evt.FeedbackId.Value,
                    Message = evt.Feedback.Message,
                    Score = evt.Feedback.Score,
                    Solved = evt.Feedback.Solved,
                    OptionalScores = evt.Feedback.OptionalScores?.MapToDictionary() ?? new Dictionary<string, int>(),
                    DateTime = evt.CreateDate
                }
            });
        }

        private void When(TicketIncomingMessageAddedEvent evt)
        {
            AddEvent(new TicketIncomingMessageEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                Message = new MessageView(evt.Message),
                Channel = evt.Channel.Value,
                CreateDate = evt.CreateDate,
            });
            Ticket.DiscussionState.LastMessageType = TicketDiscussionStateView.MessageType.Incoming;
            if (!Ticket.DiscussionState.HasAttachments && evt.Message.Attachments?.Count() > 0)
            {
                Ticket.DiscussionState.HasAttachments = true;
            }
        }

        private void When(TicketOutgoingMessageAddedEvent evt)
        {
            AddEvent(new TicketOutgoingMessageEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                Message = new MessageView(evt.Message),
                CreateDate = evt.CreateDate,
                Recipients = evt.Recipients.Select(x => new RecipientView
                {
                    Channel = x.Channel.Value,
                    UserId = x.UserId.Value,
                    DeliveryStatuses = new[]
                        {new DeliveryStatusView {Date = evt.CreateDate, Status = MessageStatus.Sending}}
                }),
                MessageId = evt.MessageId.Value
            });

            if (Ticket.DiscussionState.LastMessageType == TicketDiscussionStateView.MessageType.Incoming)
            {
                Ticket.DiscussionState.IterationCount += 1;
            }

            if (!Ticket.DiscussionState.HasAttachments && evt.Message.Attachments?.Count() > 0)
            {
                Ticket.DiscussionState.HasAttachments = true;
            }

            Ticket.DiscussionState.LastMessageType = TicketDiscussionStateView.MessageType.Outgoin;
            Ticket.DiscussionState.LastReplyDate = evt.CreateDate;
        }

        private void When(TicketMessageStatusChangedEvent evt)
        {
            var eventView = Ticket.Events.OfType<TicketOutgoingMessageEventView>()
                .First(x => x.MessageId == evt.MessageId.Value);
            var recipient = eventView.Recipients.FirstOrDefault(x => x.UserId == evt.UserId.Value);
            if (recipient == null) return;

            recipient.DeliveryStatuses = recipient.DeliveryStatuses.Concat(new DeliveryStatusView
            {
                Date = evt.CreateDate,
                Detail = evt.Detail,
                Status = evt.Status
            });
        }

        private void When(TicketLanguageChangedEvent evt)
        {
            AddEvent(new TicketLanguagesChangedEventView
            {
                Id = evt.Id,
                From = Ticket.Language,
                To = evt.Language.Value,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                CreateDate = evt.CreateDate
            });
            Ticket.Language = evt.Language.Value;
        }

        private void When(TicketLifecyclePlannedEvent evt)
        {
            AddEvent(new TicketLifecycleEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                CreateDate = evt.CreateDate,
                Type = evt.LifeCycleType,
                ExecutionDate = evt.ExecutionDate,
                Result = null
            });
        }

        private void When(TicketLifecycleExecutedEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketLifecycleEventView>()
                .FirstOrDefault(x =>
                    x.Type == evt.LifeCycleType && x.Result == null);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.Result = new ScheduledEventDoneResultView
                {Date = evt.CreateDate, Initiator = InitiatorMapper.Map(evt.Initiator)};
        }

        private void When(TicketLifecycleCanceledEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketLifecycleEventView>()
                .FirstOrDefault(x =>
                    x.Type == evt.LifeCycleType && x.Result == null);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.Result = new ScheduledEventCanceledResultView
                {Date = evt.CreateDate, Initiator = InitiatorMapper.Map(evt.Initiator)};
        }

        private void When(TicketLifecycleProlongatedEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketLifecycleEventView>()
                .FirstOrDefault(x =>
                    x.Type == evt.LifeCycleType && x.Result == null);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.ExecutionDate = evt.NextDate;
        }

        private void When(TicketNotePostedEvent evt)
        {
            var noteEvent = Ticket.Events.OfType<TicketNoteEventView>()
                .FirstOrDefault(x => x.NoteId == evt.NoteId.Value);
            if (!Ticket.DiscussionState.HasAttachments && evt.Message.Attachments?.Count() > 0)
            {
                Ticket.DiscussionState.HasAttachments = true;
            }
            if (noteEvent == null)
            {
                AddEvent(new TicketNoteEventView
                {
                    Id = evt.Id,
                    Initiator = InitiatorMapper.Map(evt.Initiator),
                    Message = new MessageView(evt.Message),
                    Tags = evt.Tags,
                    CreateDate = evt.CreateDate,
                    NoteId = evt.NoteId.Value,
                    History = new List<TicketNoteEventView.HistoryRecord>()
                });
            }
            else
            {
                noteEvent.History = noteEvent.History.Concat(new TicketNoteEventView.HistoryRecord
                    {Date = noteEvent.CreateDate, Initiator = noteEvent.Initiator, Message = noteEvent.Message});
                noteEvent.Message = new MessageView(evt.Message);
                noteEvent.Tags = evt.Tags;
            }
        }

        private void When(TicketNoteRemovedEvent evt)
        {
            var noteEvent = Ticket.Events.OfType<TicketNoteEventView>()
                .FirstOrDefault(x => x.NoteId == evt.NoteId.Value);
            if (noteEvent == null) return;

            noteEvent.DeleteTime = evt.CreateDate;
        }

        private void When(TicketUserMetaChangedEvent evt)
        {
            AddEvent(new TicketUserMetaChangedEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                New = evt.Meta,
                Old = Ticket.UserMeta.MapToDictionary(),
                CreateDate = evt.CreateDate
            });
            Ticket.UserMeta = evt.Meta.ToDictionary(x => x.Key, x => x.Value);
        }

        private void When(TicketUserIdsChangedEvent evt)
        {
            var newIds = evt.UserIds.Select(x => new UserIdInfoView
                {
                    Channel = x.Channel.Value,
                    UseForDiscussion = x.UseForDiscussion,
                    Type = x.Type,
                    UserId = x.UserId.Value
                })
                .ToList();
            AddEvent(new TicketUserIdsChangedEventView()
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                CreateDate = evt.CreateDate,
                Old = Ticket.UserIds,
                New = newIds
            });
            Ticket.UserIds = newIds;
        }

        private void When(TicketTagsChangedEvent evt)
        {
            var newTags = evt.Tags.Select(x => x.Value).ToList();
            AddEvent(new TicketTagsChangedEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                CreateDate = evt.CreateDate,
                Old = Ticket.Tags,
                New = newTags
            });
            Ticket.Tags = newTags;
        }

        private void When(TicketReminderScheduledEvent evt)
        {
            AddEvent(new TicketReminderEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                Reminder = new ReminderView(evt.Reminder),
                Result = null,
                CreateDate = evt.CreateDate
            });
        }

        private void When(TicketReminderCanceledEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketReminderEventView>()
                .FirstOrDefault(x => x.Reminder.Id == evt.ReminderId.Value);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.Result = new ScheduledEventCanceledResultView
            {
                Date = evt.CreateDate,
                Initiator = InitiatorMapper.Map(evt.Initiator)
            };
        }

        private void When(TicketReminderExecutedEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketReminderEventView>()
                .FirstOrDefault(x => x.Reminder.Id == evt.ReminderId.Value);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.Result = new ScheduledEventDoneResultView
            {
                Date = evt.CreateDate,
                Initiator = InitiatorMapper.Map(evt.Initiator)
            };
        }

        private void When(TicketApprovalStatusAddedEvent evt)
        {
            AddEvent(new TicketApprovalStatusEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                CreateDate = evt.CreateDate,
                Message = evt.Message.Text,
                State = TicketApprovalStatusEventView.ApproveState.Waiting,
                RejectId = evt.AuditId.Value,
                ForStatus = new TicketStatusView(evt.Status)
            });
        }

        private void When(TicketApprovalStatusApprovedEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketApprovalStatusEventView>()
                .FirstOrDefault(x => x.RejectId == evt.AuditId.Value);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.State = TicketApprovalStatusEventView.ApproveState.Approved;
        }

        private void When(TicketApprovalStatusCanceledEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketApprovalStatusEventView>()
                .FirstOrDefault(x => x.RejectId == evt.AuditId.Value);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.State = TicketApprovalStatusEventView.ApproveState.Canceled;
        }

        private void When(TicketApprovalStatusDeniedEvent evt)
        {
            var viewEvent = Ticket.Events.OfType<TicketApprovalStatusEventView>()
                .FirstOrDefault(x => x.RejectId == evt.AuditId.Value);
            if (viewEvent == null) return; // TODO: Error?

            viewEvent.State = TicketApprovalStatusEventView.ApproveState.Denied;
        }

        private void When(TicketUserUnsubscribedEvent evt)
        {
            AddEvent(new TicketUserUnsubscribedEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                Message = evt.Message,
                CreateDate = evt.CreateDate,
                UserId = evt.UserId.Value
            });
        }

        private void When(TicketAutoreplySendedEvent evt)
        {
            var newTags = Ticket.Tags.Concat(evt.Tags.Select(x => x.Value).ToList()).Distinct();
            AddEvent(new TicketTagsChangedEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                CreateDate = evt.CreateDate,
                Old = Ticket.Tags,
                New = newTags
            });
            Ticket.Tags = newTags;
        }

        private void When(TicketClosedEvent evt)
        {
            Ticket.Status.Kind = TicketStatusKind.Closed;
            AddEvent(new TicketClosedEventView
            {
                Id = evt.Id,
                Initiator = InitiatorMapper.Map(evt.Initiator),
                CreateDate = evt.CreateDate,
            });
        }
    }
}
