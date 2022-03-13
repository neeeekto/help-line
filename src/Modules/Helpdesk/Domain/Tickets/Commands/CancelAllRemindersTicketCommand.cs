using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class CancelAllRemindersTicketCommand : TicketCommand
    {
        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            var scheduledReminders =
                ctx.Ticket.State.Reminders.Where(x => x.Status == TicketReminderState.Statuses.Scheduled);
            foreach (var scheduledReminder in scheduledReminders)
            {
                await ctx.Execute(new CancelReminderTicketCommand(scheduledReminder.Reminder.Id));
            }
            return VoidResult.Value;
        }
    }
}
