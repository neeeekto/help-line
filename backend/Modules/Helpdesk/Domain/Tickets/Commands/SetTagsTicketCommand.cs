using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class SetTagsTicketCommand : TicketCommand
    {
        public IEnumerable<Tag> Tags { get; private set; }

        public SetTagsTicketCommand(IEnumerable<Tag> tags)
        {
            Tags = tags;
        }

        public SetTagsTicketCommand(params Tag[] tags)
        {
            Tags = tags;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            var newTags = Tags.Distinct();
            ctx.RiseEvent(new TicketTagsChangedEvent(ctx.Ticket.Id, ctx.Initiator, newTags));
            return VoidResult.Task;
        }
    }

    public class AddTagsTicketCommand : TicketCommand
    {
        public IEnumerable<Tag> Tags { get; private set; }

        public AddTagsTicketCommand(IEnumerable<Tag> tags)
        {
            Tags = tags;
        }

        public AddTagsTicketCommand(params Tag[] tags)
        {
            Tags = tags;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            var addTags = Tags.Where(x => !ctx.Ticket.State.Tags.Contains(x)).ToList();
            if (addTags.Any())
            {
                var newTags = ctx.Ticket.State.Tags.Concat(addTags);
                ctx.RiseEvent(new TicketTagsChangedEvent(ctx.Ticket.Id, ctx.Initiator, newTags));
            }

            return VoidResult.Task;
        }
    }

    public class RemoveTagsTicketCommand : TicketCommand
    {
        public IEnumerable<Tag> Tags { get; private set; }

        public RemoveTagsTicketCommand(IEnumerable<Tag> tags)
        {
            Tags = tags;
        }

        public RemoveTagsTicketCommand(params Tag[] tags)
        {
            Tags = tags;
        }

        internal override Task<VoidResult>Execute(CommandContext ctx)
        {
            ctx.CheckRule(new InitiatorMustExistRule(ctx.Initiator));
            var newTags = ctx.Ticket.State.Tags.Where(x => !Tags.Contains(x)).ToList();
            if (newTags.Count != ctx.Ticket.State.Tags.Count)
                ctx.RiseEvent(new TicketTagsChangedEvent(ctx.Ticket.Id, ctx.Initiator, newTags));
            return VoidResult.Task;
        }
    }
}
