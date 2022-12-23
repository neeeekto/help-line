using HelpLine.BuildingBlocks.Application.Utils;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TicketReopenConditionData
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int MinimalScore { get; set; }
        public bool MustSolved { get; set; }
    }

    public class TicketReopenCondition : TicketReopenConditionData
    {
        public string Id => $"{ProjectId}:{StringHasher.Make(Name.ToUpperInvariant())}";
        public string ProjectId { get; internal set; }
        public bool Enabled { get; internal set; }

    }
}
