using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.Modules.UserAccess.Domain.Roles;

namespace HelpLine.Modules.UserAccess.Infrastructure.Domain.Roles
{
    internal class RoleRepository : EntityRepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IMongoContext context, IDomainEventCollector domainEventCollector) : base(context,
            domainEventCollector)
        {
        }

        protected override Expression<Func<Role, bool>> GetIdFilter(Role entity)
        {
            return x => x.Id == entity.Id;
        }


        public Task<Role> Get(RoleId roleId)
        {
            return FindOne(x => x.Id == roleId);
        }

        Task IRoleRepository.Remove(RoleId role)
        {
            throw new NotImplementedException();
        }
    }
}
