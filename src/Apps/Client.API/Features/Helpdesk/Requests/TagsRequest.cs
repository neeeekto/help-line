using System.Collections.Generic;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class TagsRequest
    {
        public IEnumerable<string> Tags { get; set; }
        public bool Enabled { get; set; }
    }
}
