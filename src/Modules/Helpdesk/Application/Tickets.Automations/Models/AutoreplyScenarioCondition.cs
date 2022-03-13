using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models
{
    public class AutoreplyScenarioCondition
    {
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<TagCondition> TagConditions { get; set; }
        public LocalizeDictionary<string>? Keywords { get; set; }
        public bool? Attachments { get; set; } // null - not set, true - with attachments, false - without attachments

    }
}
