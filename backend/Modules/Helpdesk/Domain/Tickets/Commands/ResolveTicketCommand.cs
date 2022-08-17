using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ResolveTicketCommand : TicketCommand
    {
        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            await ctx.Execute(new ChangeStatusTicketCommand(TicketStatus.Opened(TicketStatusType.Resolved)));
            await ctx.Execute(new CheckAndAssignIfOperatorTicketCommand());
            await ctx.Execute(new CancelAllRemindersTicketCommand());
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return VoidResult.Value;
        }
    }
}
