namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;

public abstract class TicketSortBase
{
    public bool Descending { get; set; }
}

public sealed class TicketIdSort : TicketSortBase {}
 
