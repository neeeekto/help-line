using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class SearchTicketRequest
    {
        public IFilter? Filter { get; set; }
        public IEnumerable<Sort> Sorts { get; set; }
    }
}
