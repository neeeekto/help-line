using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Extends;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class AddIncomingMessageTicketCommand : TicketCommand
    {
        public Message Message { get; }
        public UserId UserId { get; }
        public Channel Channel { get; }


        public AddIncomingMessageTicketCommand(Message message, UserId userId, Channel channel)
        {
            Message = message;
            UserId = userId;
            Channel = channel;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new MessageCannotBeEmptyRule(Message));
            ctx.CheckRule(new UserIdMustExistRule(UserId,
                ctx.Ticket.State.User.Channels.Where(x => x.Channel == Channel).Select(x => x.UserId)));
            var initiator = new UserInitiator(UserId);
            ctx.RiseEvent(new TicketIncomingMessageAddedEvent(ctx.Ticket.Id, Message, UserId, Channel));

            if (ctx.Ticket.State.HasAnswer)
                await ctx.Execute(
                    new ChangeStatusTicketCommand(ctx.Ticket.State.Status.To(TicketStatusType.AwaitingReply)),
                    initiator);

            if (ctx.Ticket.IsAssigned() && !ctx.Ticket.State.HardAssigment)
                await ctx.Execute(new UnassignTicketCommand(), initiator);

            if (ctx.Ticket.State.ActiveApprovalStatusAuditId != null)
                ctx.RiseEvent(new TicketApprovalStatusCanceledEvent(ctx.Ticket.Id,
                    ctx.Ticket.State.ActiveApprovalStatusAuditId,
                    initiator));

            await ctx.Execute(new CancelAllRemindersTicketCommand(), initiator);
            await ctx.Services.UnsubscribeManager.TryRemove(UserId, ctx.Ticket.State.ProjectId);
            await ctx.Execute(new ApplyLifecycleTicketCommand());
            return VoidResult.Value;
        }
    }
}
