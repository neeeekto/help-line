using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Macros
{
    internal class ScenarioScheduleRepository : RepositoryBase<ScenarioSchedule>
    {
        public ScenarioScheduleRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<ScenarioSchedule, bool>> GetIdFilter(ScenarioSchedule entity)
        {
            return x => x.Interval == entity.Interval;
        }
    }
}
