using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Aux
{
    internal class UnsubscribeRepository : RepositoryBase<Unsubscribe>
    {
        public UnsubscribeRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<Unsubscribe, bool>> GetIdFilter(Unsubscribe entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}
