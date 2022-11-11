using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Quality.Domain.Tickets
{
    public class TicketId : TypedStringIdValueBase
    {
        public TicketId(string value) : base(value)
        {
        }
    }
}
