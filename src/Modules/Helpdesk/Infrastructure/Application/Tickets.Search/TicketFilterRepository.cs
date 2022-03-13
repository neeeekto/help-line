using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search
{
    internal class TicketFilterRepository : RepositoryBase<TicketFilter>
    {
        public TicketFilterRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<TicketFilter, bool>> GetIdFilter(TicketFilter entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}
