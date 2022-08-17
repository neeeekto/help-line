using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Queries;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsersByRole
{
    class GetUsersByRoleQueryHandler : IQueryHandler<GetUsersByRoleQuery, IEnumerable<User>>
    {
        private readonly IMongoContext _context;

        public GetUsersByRoleQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> Handle(GetUsersByRoleQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _context.GetCollection<User>().Find(x => true)
                .ToListAsync(cancellationToken: cancellationToken);
            return users.Where(x => x.Roles.Global.Contains(new RoleId(request.RoleId)) ||
                                    x.Roles.ByProject.SelectMany(x => x.Value)
                                        .Contains(new RoleId(request.RoleId))
            );
        }
    }
}