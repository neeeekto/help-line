using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Extends;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class UnassignTicketCommand : TicketCommand
    {
        async internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));

            if (ctx.Ticket.IsAssigned())
            {
                ctx.RiseEvent(new TicketAssignChangedEvent(ctx.Ticket.Id, ctx.Initiator, null));
                if (ctx.Ticket.State.HardAssigment)
                    await ctx.Execute(new ChangeHardAssigmentTicketCommand(false));
            }
            return VoidResult.Value;
        }
    }
}
