using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class TagDescriptionRequest
    {
        public IEnumerable<TagsDescriptionIssue> Issues { get; set; }
        public bool Enabled { get; set; }
    }
}
