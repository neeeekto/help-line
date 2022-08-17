using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Extends;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class RunTicketScheduleCommand : TicketCommand<bool>
    {
        public ScheduleId ScheduleId { get; private set; }

        public RunTicketScheduleCommand(ScheduleId scheduleId)
        {
            ScheduleId = scheduleId;
        }

        internal override async Task<bool> Execute(CommandContext ctx)
        {
            var scheduleIsReminderSchedule = await ctx.Execute(new TryRunRemindersScheduleTicketCommand(ScheduleId));
            var scheduleIsLifecycleSchedule = await ctx.Execute(new TryRunLifecycleScheduleTicketCommand(ScheduleId));
            return scheduleIsReminderSchedule || scheduleIsLifecycleSchedule;
        }
    }

    class TryRunRemindersScheduleTicketCommand : TicketCommand<bool>
    {
        public ScheduleId ScheduleId { get; private set; }

        public TryRunRemindersScheduleTicketCommand(ScheduleId scheduleId)
        {
            ScheduleId = scheduleId;
        }

        internal override async Task<bool> Execute(CommandContext ctx)
        {
            var scheduledReminders = ctx.Ticket.State.Reminders
                .Where(x => x.ScheduleId == ScheduleId && x.Status == TicketReminderState.Statuses.Scheduled).ToList();
            if (!scheduledReminders.Any())
                return false; // It is not our schedule!

            if (!ctx.Ticket.State.User.Channels.Any())
            {
                var initiator = new SystemInitiator("No channels");
                foreach (var scheduledReminder in scheduledReminders)
                    ctx.RiseEvent(new TicketReminderCanceledEvent(ctx.Ticket.Id, initiator,
                        scheduledReminder.Reminder.Id));
                await ctx.Services.Scheduler.Cancel(ScheduleId); // Close Scheduler
                return true;
            }

            foreach (var scheduledReminder in scheduledReminders)
            {
                await Execute(ctx, scheduledReminder.Reminder, scheduledReminder.Initiator);
                ctx.RiseEvent(new TicketReminderExecutedEvent(ctx.Ticket.Id, new SystemInitiator(),
                    scheduledReminder.Reminder.Id));
            }

            await ctx.Services.Scheduler.Cancel(ScheduleId); // Close Scheduler

            return true;
        }

        private async Task Execute(CommandContext ctx, TicketReminder reminder, Initiator initiator)
        {
            switch (reminder)
            {
                case TicketFinalReminder finalReminder:
                {
                    if (finalReminder.Resolve)
                    {
                        await ctx.Execute(new SendMessageTicketCommand(reminder.Message), initiator);
                        ctx.PublishStatusChangedEvent(TicketStatus.Opened(TicketStatusType.Resolved));
                        await ctx.Execute(new CheckAndAssignIfOperatorTicketCommand(), initiator);
                        await ctx.Execute(new ApplyLifecycleTicketCommand());
                    }
                    else
                    {
                        await ctx.Execute(new AddOutgoingMessageTicketCommand(finalReminder.Message), initiator);
                    }

                    break;
                }
                case TicketSequentialReminder sequentialReminder:
                {
                    await ctx.Execute(new SendMessageTicketCommand(reminder.Message), initiator);
                    await ctx.Execute(
                        new ChangeStatusTicketCommand(ctx.Ticket.State.Status.To(TicketStatusType.Answered)),
                        new SystemInitiator());

                    await ctx.Execute(new AddReminderTicketCommand(sequentialReminder.Next), initiator);
                    break;
                }
            }
        }
    }

    class TryRunLifecycleScheduleTicketCommand : TicketCommand<bool>
    {
        public ScheduleId ScheduleId { get; private set; }

        public TryRunLifecycleScheduleTicketCommand(ScheduleId scheduleId)
        {
            ScheduleId = scheduleId;
        }

        internal override async Task<bool> Execute(CommandContext ctx)
        {
            if (ctx.Ticket.State.LifecycleStatus.All(x => x.Value != ScheduleId)) return false;

            var (key, _) = ctx.Ticket.State.LifecycleStatus.SingleOrDefault(x => x.Value == ScheduleId);
            switch (key)
            {
                case TicketLifeCycleType.Feedback:
                    await HandleFeedback(ctx, ScheduleId);
                    break;
                case TicketLifeCycleType.Resolving:
                    await HandleResolving(ctx, ScheduleId);
                    break;
                case TicketLifeCycleType.Closing:
                    await HandleClosing(ctx, ScheduleId);
                    break;
                default:
                    throw new ArgumentException("Unknown lifecycle");
            }

            return true;
        }

        private async Task HandleFeedback(CommandContext ctx,
            ScheduleId scheduleId)
        {
            await MarkExecuted(ctx, scheduleId, TicketLifeCycleType.Feedback);
            await ctx.Execute(new SendFeedbackTicketCommand(), new SystemInitiator());
        }

        private async Task HandleClosing(CommandContext ctx,
            ScheduleId scheduleId)
        {
            if (await ProlongateIfActivity(ctx, scheduleId, TicketLifeCycleType.Closing))
                return;
            // TODO: Странно отменять фидбек если он не отправлен вообще конечно же
            // Отменяем фидбек таймер если он запланирован, так как закрытый тикет оценивать нельзя
            await ctx.Execute(new TryCancelLifeCycleTicketCommand(TicketLifeCycleType.Feedback), new SystemInitiator());
            await MarkExecuted(ctx, scheduleId, TicketLifeCycleType.Closing);
            ctx.RiseEvent(new TicketClosedEvent(ctx.Ticket.Id, new SystemInitiator()));
        }

        private async Task HandleResolving(CommandContext ctx,
            ScheduleId scheduleId)
        {
            if (await ProlongateIfActivity(ctx, scheduleId, TicketLifeCycleType.Resolving))
                return;

            await MarkExecuted(ctx, scheduleId, TicketLifeCycleType.Resolving);
            await ctx.Execute(new ResolveTicketCommand(), new SystemInitiator());
        }

        private bool CheckActivity(CommandContext ctx, TimeSpan delay)
        {
            return DateTime.UtcNow - ctx.Ticket.State.LastOperatorActivity < delay;
        }

        private async Task<bool> ProlongateIfActivity(CommandContext ctx,
            ScheduleId scheduleId, TicketLifeCycleType type)
        {
            var delay = await ctx.Services.Configurations.GetInactivityDelay(ctx.Ticket.State.ProjectId);
            if (!CheckActivity(ctx, delay)) return false;

            var nextDate = DateTime.UtcNow.Add(delay);
            await ctx.Services.Scheduler.Prolong(nextDate, ctx.Ticket.Id, scheduleId);
            ctx.RiseEvent(new TicketLifecycleProlongatedEvent(ctx.Ticket.Id, new SystemInitiator(), type,
                scheduleId, nextDate));
            return true;
        }

        private async Task MarkExecuted(CommandContext ctx,
            ScheduleId scheduleId, TicketLifeCycleType type)
        {
            ctx.RiseEvent(new TicketLifecycleExecutedEvent(ctx.Ticket.Id, new SystemInitiator(), ScheduleId,
                type));
            await ctx.Services.Scheduler.Cancel(scheduleId); // Close Scheduler
        }
    }
}
