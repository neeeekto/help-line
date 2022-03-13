using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Macros
{
    internal class ScenarioRepository : RepositoryBase<Scenario>, IScenarioRepository
    {
        public ScenarioRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<Scenario, bool>> GetIdFilter(Scenario entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}
