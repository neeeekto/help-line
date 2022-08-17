using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models
{
    public class EmailChannelSettings : ChannelSettings
    {
        public override string Key => Channels.Email;
        public FromSettings From { get; set; }

        public IEnumerable<ConditionalTemplate> Templates { get; set; }

        public class ConditionalTemplate
        {
            public int Weight { get; set; }
            public IEnumerable<TagCondition> Conditions { get; set; }
            public EmailTemplate Discussion { get; set; }
            public EmailTemplate Feedback { get; set; }
        }

        public class FromSettings
        {
            public string Domain { get; set; }
            public string Name { get; set; }
        }
    }
}
