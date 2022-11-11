using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Aux
{
    internal class TicketReopenConditionRepository : RepositoryBase<TicketReopenCondition>
    {
        public TicketReopenConditionRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<TicketReopenCondition, bool>> GetIdFilter(TicketReopenCondition entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}
