using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class ChangeLanguageTicketCommand : TicketCommand
    {
        public LanguageCode Language { get; private set; }

        public ChangeLanguageTicketCommand(LanguageCode language)
        {
            Language = language;
        }

        internal override async Task<VoidResult> Execute(CommandContext ctx)
        {
            await ctx.CheckRule(new LanguageIsExistRule(ctx.Ticket.State.ProjectId, Language, ctx.Services.Checker));
            ctx.RiseEvent(new TicketLanguageChangedEvent(ctx.Ticket.Id, ctx.Initiator, Language));
            return VoidResult.Value;
        }
    }
}
