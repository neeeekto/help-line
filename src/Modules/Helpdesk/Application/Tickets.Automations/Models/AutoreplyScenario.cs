using HelpLine.BuildingBlocks.Application.Utils;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models
{
    public class AutoreplyScenario
    {
        public string Id => $"{ProjectId}:{StringHasher.Make(Name.ToUpperInvariant())}"; // composite index
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public bool Enabled { get; set; }
        public int Weight { get; set; }
        public AutoreplyScenarioCondition Condition { get; set; }
        public AutoreplyScenarioAction Action { get; set; }
    }
}
