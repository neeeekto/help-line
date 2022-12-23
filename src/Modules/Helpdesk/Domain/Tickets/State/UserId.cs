using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class UserId : TypedStringIdValueBase
    {
        public UserId(string value) : base(value)
        {
        }
    }
}
