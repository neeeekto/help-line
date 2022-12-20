using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Infrastructure.Domain.Users
{
    internal class UserRepository : EntityRepositoryBase<User>, IUserRepository
    {
        public UserRepository(IMongoContext context, IDomainEventCollector eventCollector) : base(context,
            eventCollector)
        {
        }

        protected override Expression<Func<User, bool>> GetIdFilter(User entity)
        {
            return x => x.Id == entity.Id;
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return FindOne(x => x.Email == email);
        }

        public Task<User> Get(UserId userId)
        {
            return FindOne(x => x.Id == userId);
        }

        public Task<IEnumerable<User>> Get(IEnumerable<UserId> userIds)
        {
            return Find(x => userIds.Contains(x.Id));
        }
    }
}
