using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class AddReminderTicketCommand : TicketCommand<TicketReminderId>
    {
        public TicketReminder Reminder { get; private set; }

        public AddReminderTicketCommand(TicketReminder reminder)
        {
            Reminder = reminder;
        }

        internal override async Task<TicketReminderId> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new TicketCheckStatusRule(
                status => !status.In(TicketStatusType.ForReject, TicketStatusType.Resolved),
                ctx.Ticket.State));

            if (ctx.Initiator is OperatorInitiator operatorInitiator)
                await ctx.Execute(new AssignTicketCommand(operatorInitiator.OperatorId), new SystemInitiator());

            var scheduleId = new ScheduleId();
            var date = DateTime.UtcNow.Add(Reminder.Delay);
            await ctx.Services.Scheduler.Schedule(date, ctx.Ticket.Id, scheduleId);
            ctx.RiseEvent(new TicketReminderScheduledEvent(ctx.Ticket.Id, ctx.Initiator, scheduleId, Reminder, date));
            return Reminder.Id;
        }
    }
}
