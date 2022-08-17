using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Aux
{
    internal class BanRepository : RepositoryBase<Ban>
    {
        public BanRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<Ban, bool>> GetIdFilter(Ban entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}
