using System;
using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using Microsoft.CSharp.RuntimeBinder;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketState : IEventsSourcingAggregateState
    {
        public TicketStatus Status { get; private set; }
        public TicketStatus? FallbackStatus { get; private set; }
        public TicketAuditId? ActiveApprovalStatusAuditId { get; private set; }
        public ProjectId ProjectId { get; private set; }
        public TicketPriority Priority { get; private set; }
        public OperatorId? AssignedTo { get; private set; }
        public bool HardAssigment { get; private set; }
        public bool HasAnswer { get; private set; }
        public User User { get; private set; }

        private Dictionary<TicketLifeCycleType, ScheduleId> _lifecycleStatus;
        public IReadOnlyDictionary<TicketLifeCycleType, ScheduleId> LifecycleStatus => _lifecycleStatus;

        private List<TicketReminderState> _reminders;
        public IReadOnlyCollection<TicketReminderState> Reminders => _reminders.AsReadOnly();

        private Dictionary<TicketNoteId, TicketNote> _notes;
        public IReadOnlyDictionary<TicketNoteId, TicketNote> Notes => _notes;

        private List<Tag> _tags;
        public IReadOnlyCollection<Tag> Tags => _tags;

        private List<TicketOutgoingMessage> _outgoingMessages;
        public IReadOnlyCollection<TicketOutgoingMessage> OutgoingMessages => _outgoingMessages.AsReadOnly();

        private Dictionary<TicketFeedbackId, DateTime?> _feedbacks;
        public IReadOnlyDictionary<TicketFeedbackId, DateTime?> Feedbacks => _feedbacks;
        public DateTime LastOperatorActivity { get; private set; }
        public bool NeedSendFeedback { get; private set; }

        internal TicketState()
        {
        }

        private static void Apply(EventBase<TicketId> evt) // For ignore
        {
        }

        internal void ApplyEvent(EventBase<TicketId> evt)
        {
            Apply((dynamic) evt);
        }

        private void Apply(TicketCreatedEvent evt)
        {
            User = new User(evt.UserMeta, evt.UserChannels);
            Priority = evt.Priority;
            Status = evt.Status;
            FallbackStatus = GetFallbackStatus(evt.Status);
            HardAssigment = false;
            HasAnswer = false;
            ProjectId = evt.ProjectId;
            LastOperatorActivity = evt.CreateDate; // TODO!!!!!!! Определять дату последней работы с тикетом!
            NeedSendFeedback = evt.Initiator is UserInitiator;
            _notes = new Dictionary<TicketNoteId, TicketNote>();
            _tags = evt.Tags.ToList();
            _outgoingMessages = new List<TicketOutgoingMessage>();
            _reminders = new List<TicketReminderState>();
            _lifecycleStatus = new Dictionary<TicketLifeCycleType, ScheduleId>();
            _feedbacks = new Dictionary<TicketFeedbackId, DateTime?>();
        }

        private void Apply(TicketAssingmentBindingChangedEvent evt)
        {
            HardAssigment = evt.HardAssigment;
            LastOperatorActivity = evt.CreateDate;
        }

        private void Apply(TicketClosedEvent evt)
        {
            Status = TicketStatus.Closed(Status.Type);
        }

        private void Apply(TicketAssignChangedEvent evt)
        {
            AssignedTo = evt.Assignee;
        }

        private void Apply(TicketIncomingMessageAddedEvent evt)
        {
        }

        private void Apply(TicketMessageStatusChangedEvent evt)
        {
            var message = _outgoingMessages.Find(x => x.Id == evt.MessageId);
            message?.ChangeStatus(evt.UserId, evt.Status);
        }

        private void Apply(TicketLanguageChangedEvent evt)
        {
        }

        private void Apply(TicketTagsChangedEvent evt)
        {
            _tags = evt.Tags.ToList();
        }

        private void Apply(TicketOutgoingMessageAddedEvent evt)
        {
            _outgoingMessages.Add(new TicketOutgoingMessage(evt.MessageId, evt.Message, evt.Initiator, evt.Recipients));
            HasAnswer = true;
        }


        private void Apply(TicketPriorityChangedEvent evt)
        {
            Priority = evt.Priority;
        }

        private void Apply(TicketStatusChangedEvent evt)
        {
            Status = evt.Status;
            FallbackStatus = GetFallbackStatus(evt.Status);
        }

        private void Apply(TicketNotePostedEvent evt)
        {
            _notes[evt.NoteId] = new TicketNote(evt.NoteId, evt.Message, evt.Tags);
        }

        private void Apply(TicketNoteRemovedEvent evt)
        {
            _notes.Remove(evt.NoteId);
        }


        private void Apply(TicketAutoreplySendedEvent evt)
        {
            if (evt.Tags.Any())
            {
                _tags.AddRange(evt.Tags);
                _tags = _tags.Distinct().ToList();
            }
        }

        private void Apply(TicketLifecyclePlannedEvent evt)
        {
            _lifecycleStatus.Add(evt.LifeCycleType, evt.ScheduleId);
        }

        private void Apply(TicketLifecycleCanceledEvent evt)
        {
            _lifecycleStatus.Remove(evt.LifeCycleType);
        }

        private void Apply(TicketLifecycleExecutedEvent evt)
        {
            _lifecycleStatus.Remove(evt.LifeCycleType);
        }

        private void Apply(TicketLifecycleProlongatedEvent evt)
        {
            _lifecycleStatus.TryAdd(evt.LifeCycleType, evt.ScheduleId);
        }

        private void Apply(TicketReminderScheduledEvent evt)
        {
            _reminders.Add(new TicketReminderState(evt.ScheduleId, evt.Reminder, TicketReminderState.Statuses.Scheduled,
                evt.Initiator));
        }

        private void Apply(TicketReminderExecutedEvent evt)
        {
            var reminder = _reminders.FirstOrDefault(x => x.Reminder.Id == evt.ReminderId);
            reminder.Status = TicketReminderState.Statuses.Executed;
        }

        private void Apply(TicketReminderCanceledEvent evt)
        {
            var reminder = _reminders.FirstOrDefault(x => x.Reminder.Id == evt.ReminderId);
            reminder.Status = TicketReminderState.Statuses.Canceled;
        }

        private void Apply(TicketFeedbackSentEvent evt)
        {
            _feedbacks.Add(evt.FeedbackId, null);
        }

        private void Apply(TicketFeedbackAddedEvent evt)
        {
            if (_feedbacks.TryGetValue(evt.FeedbackId, out var lastDate) && lastDate == null)
            {
                _feedbacks[evt.FeedbackId] = evt.CreateDate;
            }
        }

        private void Apply(TicketUserIdsChangedEvent evt)
        {
            User.Set(evt.UserIds);
        }

        private void Apply(TicketApprovalStatusAddedEvent evt)
        {
            ActiveApprovalStatusAuditId = evt.AuditId;
        }

        private void Apply(TicketApprovalStatusCanceledEvent evt)
        {
            ActiveApprovalStatusAuditId = null;
        }

        private void Apply(TicketApprovalStatusApprovedEvent evt)
        {
            ActiveApprovalStatusAuditId = null;
        }

        private void Apply(TicketApprovalStatusDeniedEvent evt)
        {
            ActiveApprovalStatusAuditId = null;
        }

        private void Apply(TicketUserMetaChangedEvent evt)
        {
            User.Meta = evt.Meta;
        }

        private TicketStatus GetFallbackStatus(TicketStatus status) =>
            status.In(TicketStatusType.Resolved, TicketStatusType.Rejected, TicketStatusType.ForReject)
                ? FallbackStatus ?? status.To(TicketStatusType.New)
                : status;
    }
}
