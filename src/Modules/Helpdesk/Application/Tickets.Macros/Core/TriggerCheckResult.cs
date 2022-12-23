using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core
{
    internal class TriggerCheckResult
    {

        public static TriggerCheckResult MakeSuccess(IEnumerable<TicketId>? onlyFor = null)
        {
            return new TriggerCheckResult(true, onlyFor);
        }

        public static TriggerCheckResult MakeFail()
        {
            return new TriggerCheckResult(false);
        }

        public bool Success { get; set; }
        public IEnumerable<TicketId>? OnlyFor { get; set; }

        public TriggerCheckResult(bool success, IEnumerable<TicketId>? onlyFor = null)
        {
            Success = success;
            OnlyFor = onlyFor;
        }


    }
}
