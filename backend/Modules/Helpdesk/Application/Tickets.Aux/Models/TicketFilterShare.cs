using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public abstract class TicketFilterShareBase
    {
    }

    public class TicketFilterShareGlobal : TicketFilterShareBase
    {
    }

    public class TicketFilterShareForOperators : TicketFilterShareBase
    {
        public IEnumerable<Guid> Operators { get; set; }
    }
}
