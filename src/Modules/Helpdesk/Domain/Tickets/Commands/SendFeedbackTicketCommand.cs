using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class SendFeedbackTicketCommand : TicketCommand<TicketFeedbackId?>
    {
        internal override async Task<TicketFeedbackId?> Execute(CommandContext ctx)
        {
            if (!ctx.Ticket.State.User.Channels.Any()) return null; // We do not do anything
            var feedbackId = new TicketFeedbackId();
            await ctx.Services.FeedbackDispatcher.Enqueue(ctx.Ticket.Id, feedbackId, ctx.Ticket.State.User.Channels, ctx.Ticket.State.ProjectId);
            ctx.RiseEvent(new TicketFeedbackSentEvent(ctx.Ticket.Id, feedbackId, ctx.Initiator));
            return feedbackId;
        }
    }
}
