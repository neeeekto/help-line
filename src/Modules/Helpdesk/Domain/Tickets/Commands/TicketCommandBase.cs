using System.Threading.Tasks;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Commands
{
    public class VoidResult
    {
        public static VoidResult Value = new VoidResult();
        public static Task<VoidResult> Task = System.Threading.Tasks.Task.FromResult(Value);
    }

    public abstract class TicketCommandBase {}
    public abstract class TicketCommand<TResult> : TicketCommandBase
    {
        internal abstract Task<TResult> Execute(CommandContext ctx);
    }

    public abstract class TicketCommand : TicketCommand<VoidResult>
    {
    }
}
