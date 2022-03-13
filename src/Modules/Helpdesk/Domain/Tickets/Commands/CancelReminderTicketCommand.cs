using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class CancelReminderTicketCommand : TicketCommand
    {
        public TicketReminderId ReminderId { get; private set; }

        public CancelReminderTicketCommand(TicketReminderId reminderId)
        {
            ReminderId = reminderId;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketReminderShouldBeInStatusRule(ctx.Ticket.State, ReminderId,
                TicketReminderState.Statuses.Scheduled));

            var reminderState = ctx.Ticket.State.Reminders.FirstOrDefault(x => x.Reminder.Id == ReminderId);
            await ctx.Services.Scheduler.Cancel(reminderState.ScheduleId);
            ctx.RiseEvent(new TicketReminderCanceledEvent(ctx.Ticket.Id, ctx.Initiator, ReminderId));
            return VoidResult.Value;
        }
    }
}
