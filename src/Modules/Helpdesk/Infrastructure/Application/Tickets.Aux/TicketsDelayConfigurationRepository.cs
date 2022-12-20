using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Aux
{
    internal class TicketsDelayConfigurationRepository : RepositoryBase<TicketsDelayConfiguration>
    {
        public TicketsDelayConfigurationRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<TicketsDelayConfiguration, bool>> GetIdFilter(
            TicketsDelayConfiguration entity)
        {
            return x => x.ProjectId == entity.ProjectId;
        }
    }
}
