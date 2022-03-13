using System;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Identity;

namespace HelpLine.Modules.UserAccess.Infrastructure.Application.Identity
{
    internal class UserSessionRepository : RepositoryBase<UserSession>
    {
        public UserSessionRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<UserSession, bool>> GetIdFilter(UserSession entity)
        {
            return x => x.UserId == entity.UserId;
        }
    }
}
