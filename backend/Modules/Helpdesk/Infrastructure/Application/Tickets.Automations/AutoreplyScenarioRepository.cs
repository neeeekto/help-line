using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Automations
{
    internal class AutoreplyScenarioRepository : RepositoryBase<AutoreplyScenario>
    {
        public AutoreplyScenarioRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<AutoreplyScenario, bool>> GetIdFilter(AutoreplyScenario entity) =>
            x => x.Id == entity.Id;
    }
}
