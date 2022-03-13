using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TagsDescription
    {
        private string _id => $"{ProjectId}:{Tag}";
        public string Tag { get; internal set; }
        public string ProjectId { get; internal set; }
        public bool Enabled { get; internal set; }
        public IEnumerable<TagsDescriptionIssue> Issues { get; internal set; }
    }

    public class TagsDescriptionIssue
    {
        public LocalizeDictionary<TagsDescriptionIssueContent> Contents { get; set; }

        public IEnumerable<Guid> Audience { get; set; } // Operator role in HD
    }

    public class TagsDescriptionIssueContent
    {
        public string Text { get; set; }
        public string Uri { get; set; }
    }
}
