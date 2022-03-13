using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketFilters
{
    public class GetTicketFiltersQuery : QueryBase<IEnumerable<TicketFilter>>
    {
        public string ProjectId { get; }
        public Guid? ForOperatorId { get; }
        public IEnumerable<TicketFilterFeatures>? Features { get; }

        public GetTicketFiltersQuery(string projectId, Guid? forOperatorId = null, IEnumerable<TicketFilterFeatures>? features = null)
        {
            Features = features;
            ProjectId = projectId;
            ForOperatorId = forOperatorId;
        }
    }
}
