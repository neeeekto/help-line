using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets
{
    public class TicketId : TypedStringIdValueBase
    {
        public TicketId(string value) : base(value)
        {
        }
    }
}
