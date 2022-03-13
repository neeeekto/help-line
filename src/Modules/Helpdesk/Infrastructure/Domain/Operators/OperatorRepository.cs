using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Operators.Contracts;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Operators
{
    internal class OperatorRepository : EntityRepositoryBase<Operator>, IOperatorRepository
    {
        public OperatorRepository(IMongoContext context, IDomainEventCollector eventCollector) : base(context,
            eventCollector)
        {
        }

        public Task<Operator> Get(OperatorId id)
        {
            return FindOne(x => x.Id == id);
        }

        protected override Expression<Func<Operator, bool>> GetIdFilter(Operator entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}
