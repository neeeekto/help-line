using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Utils
{
    public class TicketFilterCtxDescription : Description
    {
        public TicketFilterCtxDescription() : base(new DescriptionClassMap[]
        {
            new TicketFilterCtxMap()
        }, typeof(TicketFilterCtx))
        {
        }

        private class TicketFilterCtxMap : DescriptionClassMap<TicketFilterCtx>
        {
            public override void Init()
            {
                MapField(x => x.CurrentUser).SetName("Current User")
                    .SetDescription("User that will execute this filter");

                MapField(x => x.Now)
                    .SetDescription("Current date and time when filter will execute");
            }
        }
    }
}
