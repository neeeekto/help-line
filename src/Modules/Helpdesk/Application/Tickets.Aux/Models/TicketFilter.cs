using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{

    public class TicketFilterData
    {
        public string Name { get; set; }
        public IFilter Filter { get; set; }
        public TicketFilterShareBase? Share { get; set; } // share model, null for me, other - see name of class
        public IEnumerable<string> Features { get; set; } // for client features, eg: default, important, any, backend doesnt know about it and doesnt use
    }
    public class TicketFilter : TicketFilterData
    {
        public Guid Id { get; internal set; }
        public string ProjectId { get; internal set; }
        public DateTime Changed { get; internal set; }
        public Guid? Owner { get; internal set; } // null is system, GUid - operator ID
    }
}
