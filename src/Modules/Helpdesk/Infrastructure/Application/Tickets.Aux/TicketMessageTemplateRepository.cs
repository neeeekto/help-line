using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Aux
{
    internal class TicketMessageTemplateRepository : RepositoryBase<TicketMessageTemplate>
    {
        public TicketMessageTemplateRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<TicketMessageTemplate, bool>> GetIdFilter(TicketMessageTemplate entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}
