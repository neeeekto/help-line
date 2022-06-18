namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketAttachmentFilter : TicketFilterBase
{
    public bool Value { get; set; } // true - has attach, false - without attachments
}