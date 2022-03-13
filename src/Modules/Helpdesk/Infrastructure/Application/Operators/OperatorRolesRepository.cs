using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Operators
{
    public class OperatorRolesRepository : RepositoryBase<OperatorRole>
    {
        public OperatorRolesRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<OperatorRole, bool>> GetIdFilter(OperatorRole entity)
        {
            return (role) => role.Id == entity.Id;
        }
    }
}
