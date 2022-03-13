using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class Tag : TypedStringIdValueBase
    {
        public Tag(string value) : base(value)
        {
        }
    }
}
