namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketAttachmentFilter : TicketFilterBase
{
    public bool Value { get; set; } // true - has attach, false - without attachments
}