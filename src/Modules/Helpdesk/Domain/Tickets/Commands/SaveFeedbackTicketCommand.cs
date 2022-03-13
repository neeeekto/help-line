using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Extends;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class SaveFeedbackTicketCommand : TicketCommand
    {
        public TicketFeedbackId FeedbackId { get; private set; }
        public TicketFeedback Feedback { get; private set; }
        public UserId UserId { get; private set; }

        public SaveFeedbackTicketCommand(TicketFeedbackId feedbackId, TicketFeedback feedback, UserId userId)
        {
            FeedbackId = feedbackId;
            Feedback = feedback;
            UserId = userId;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            ctx.CheckRule(new TicketFeedbackMustExistRule(FeedbackId, ctx.Ticket.State));
            await ctx.CheckRule(
                new TicketFeedbackMustBeAvailableForModificationRule(ctx.Services.Configurations, ctx.Ticket.State, FeedbackId));

            ctx.RiseEvent(new TicketFeedbackAddedEvent(ctx.Ticket.Id, new UserInitiator(UserId), FeedbackId, Feedback));
            var needReopen = await ctx.Services.ReopenChecker.CheckBy(Feedback, ctx.Ticket.State.ProjectId);
            if (needReopen)
            {
                var initiator = new SystemInitiator("Bad Feedback");
                ctx.PublishStatusChangedEvent(TicketStatus.Opened(TicketStatusType.AwaitingReply), initiator);
                if (ctx.Ticket.IsAssigned() && !ctx.Ticket.State.HardAssigment)
                    await ctx.Execute(new UnassignTicketCommand(), initiator);
                await ctx.Execute(new ApplyLifecycleTicketCommand(), initiator);
            }
            return VoidResult.Value;
        }
    }
}
