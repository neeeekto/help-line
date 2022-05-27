using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{

    public class TicketSavedFilterData
    {
        public string Name { get; set; }
        public TicketFilterBase Filter { get; set; }
        public TicketFilterShareBase? Share { get; set; } // share model, null for me, other - see name of class
        public IEnumerable<string> Features { get; set; } // for client features, eg: default, important, any, backend doesnt know about it and doesnt use
        public double Order { get; set; }
    }
    public class TicketSavedFilter : TicketSavedFilterData
    {
        public Guid Id { get; internal set; }
        public string ProjectId { get; internal set; }
        public DateTime Changed { get; internal set; }
        public Guid? Owner { get; internal set; } // null is system, GUid - operator ID
    }
}
