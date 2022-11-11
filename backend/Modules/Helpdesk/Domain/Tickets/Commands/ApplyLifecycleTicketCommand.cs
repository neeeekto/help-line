using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    class TryScheduleLifeCycleTicketCommand : TicketCommand
    {
        public IEnumerable<TicketLifeCycleType> LifeCycleTypes { get; private set; }

        public TryScheduleLifeCycleTicketCommand(params TicketLifeCycleType[] lifeCycleTypes)
        {
            LifeCycleTypes = lifeCycleTypes;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            ctx.CheckRule(new TicketShouldNotBeClosedRule(ctx.Ticket.State));
            foreach (var lifeCycleType in LifeCycleTypes)
            {
                if (ctx.Ticket.State.LifecycleStatus.ContainsKey(lifeCycleType)) continue; // Skip, avoid duplications

                var delay = await ctx.Services.Configurations.GetLifeCycleDelay(ctx.Ticket.State.ProjectId,
                    lifeCycleType);
                var scheduleId = new ScheduleId();
                var date = DateTime.UtcNow.Add(delay);
                await ctx.Services.Scheduler.Schedule(date, ctx.Ticket.Id, scheduleId);
                ctx.RiseEvent(new TicketLifecyclePlannedEvent(ctx.Ticket.Id, ctx.Initiator, scheduleId, lifeCycleType,
                    date));
            }

            return VoidResult.Value;
        }
    }

    class TryCancelLifeCycleTicketCommand : TicketCommand
    {
        public IEnumerable<TicketLifeCycleType> LifeCycleTypes { get; private set; }

        public TryCancelLifeCycleTicketCommand(params TicketLifeCycleType[] lifeCycleTypes)
        {
            LifeCycleTypes = lifeCycleTypes;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            foreach (var lifeCycleType in LifeCycleTypes)
            {
                if (ctx.Ticket.State.LifecycleStatus.TryGetValue(lifeCycleType, out var scheduleId))
                {
                    await ctx.Services.Scheduler.Cancel(scheduleId);
                    ctx.RiseEvent(new TicketLifecycleCanceledEvent(ctx.Ticket.Id, ctx.Initiator, scheduleId,
                        lifeCycleType));
                }
            }

            return VoidResult.Value;
        }
    }


    internal class ApplyLifecycleTicketCommand : TicketCommand
    {
        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            if (ctx.Ticket.State.Status.In(TicketStatusKind.Pending))
            {
                await ctx.Execute(
                    new TryCancelLifeCycleTicketCommand(TicketLifeCycleType.Closing,
                        TicketLifeCycleType.Feedback,
                        TicketLifeCycleType.Resolving),
                    new SystemInitiator("Activated pending state"));
            }
            else
            {
                switch (ctx.Ticket.State.Status.Type)
                {
                    case TicketStatusType.AwaitingReply:
                        await ctx.Execute(
                            new TryCancelLifeCycleTicketCommand(TicketLifeCycleType.Closing,
                                TicketLifeCycleType.Feedback,
                                TicketLifeCycleType.Resolving),
                            new SystemInitiator());
                        break;
                    case TicketStatusType.Answered:
                        await ctx.Execute(
                            new TryCancelLifeCycleTicketCommand(TicketLifeCycleType.Closing,
                                TicketLifeCycleType.Feedback),
                            new SystemInitiator());
                        await ctx.Execute(new TryScheduleLifeCycleTicketCommand(TicketLifeCycleType.Resolving),
                            new SystemInitiator());
                        break;
                    case TicketStatusType.Resolved:
                        await ctx.Execute(
                            new TryCancelLifeCycleTicketCommand(TicketLifeCycleType.Resolving),
                            new SystemInitiator());
                        var steps = new List<TicketLifeCycleType> {TicketLifeCycleType.Closing};
                        if (ctx.Ticket.State.NeedSendFeedback) steps.Add(TicketLifeCycleType.Feedback);
                        await ctx.Execute(new TryScheduleLifeCycleTicketCommand(steps.ToArray()),
                            new SystemInitiator());
                        break;
                    case TicketStatusType.Rejected:
                    case TicketStatusType.ForReject:
                        await ctx.Execute(
                            new TryCancelLifeCycleTicketCommand(TicketLifeCycleType.Resolving,
                                TicketLifeCycleType.Closing, TicketLifeCycleType.Feedback),
                            new SystemInitiator());
                        break;
                }
            }
            return VoidResult.Value;
        }
    }
}
