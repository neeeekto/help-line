using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Extends;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    internal class ExecuteCreationHookTicketCommand : TicketCommand
    {
        public TicketCreatedEvent CreatedEvent { get; private set; }

        public ExecuteCreationHookTicketCommand(TicketCreatedEvent createdEvent)
        {
            CreatedEvent = createdEvent;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            switch (CreatedEvent.Initiator)
            {
                case OperatorInitiator operatorInitiator:
                {
                    if (!string.IsNullOrEmpty(CreatedEvent.Message?.Text))
                        await ctx.Execute(new SendMessageTicketCommand(CreatedEvent.Message), CreatedEvent.Initiator);

                    await ctx.Execute(new AssignTicketCommand(operatorInitiator.OperatorId), new SystemInitiator());
                    await ctx.Execute(new ChangeHardAssigmentTicketCommand(true), new SystemInitiator());
                    break;
                }
                case UserInitiator _:
                    var autoreply = await ctx.Services.Autoreplies.Get(CreatedEvent);
                    if (autoreply != null)
                    {
                        var initiator = new SystemInitiator("Autoreply",
                            new Dictionary<string, string> {{"id", autoreply.Id}});
                        await ctx.Execute(new SendMessageTicketCommand(autoreply.Message), initiator);
                        if (autoreply.Resolve)
                        {
                            ctx.PublishStatusChangedEvent(
                                TicketStatus.Opened(TicketStatusType.Resolved), initiator);
                            await ctx.Execute(new ApplyLifecycleTicketCommand(), initiator);
                        }
                        else if (autoreply.Reminder != null)
                            await ctx.Execute(new AddReminderTicketCommand(autoreply.Reminder), initiator);

                        ctx.RiseEvent(new TicketAutoreplySendedEvent(ctx.Ticket.Id, initiator, autoreply.Message,
                            autoreply.Resolve,
                            autoreply.Tags, autoreply.Reminder));
                    }

                    break;
            }

            return VoidResult.Value;
        }
    }
}
