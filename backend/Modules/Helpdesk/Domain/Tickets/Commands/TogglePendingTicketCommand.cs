using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class TogglePendingTicketCommand : TicketCommand
    {
        public bool Enabled { get; private set; }

        public TogglePendingTicketCommand(bool enabled)
        {
            Enabled = enabled;
        }


        internal override  async  Task<VoidResult> Execute(CommandContext ctx)
        {
            if (ctx.Ticket.State.Status.In(TicketStatusKind.Pending) == Enabled)
                return VoidResult.Value;

            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            if (Enabled)
                ctx.CheckRule(new TicketMustHaveNotResolveReminderRule(ctx.Ticket.State.Reminders));

            await ctx.Execute(new ChangeStatusTicketCommand(
                ctx.Ticket.State.Status.To(Enabled ? TicketStatusKind.Pending : TicketStatusKind.Opened)));
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return VoidResult.Value;
        }
    }
}
