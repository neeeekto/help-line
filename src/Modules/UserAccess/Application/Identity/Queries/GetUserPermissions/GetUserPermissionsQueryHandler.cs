using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Queries;
using HelpLine.Modules.UserAccess.Application.Identity.Views;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserPermissions
{
    class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionsQuery, UserPermissionsView>
    {
        private readonly IMongoContext _context;

        public GetUserPermissionsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<UserPermissionsView> Handle(GetUserPermissionsQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _context.GetCollection<User>().Find(x => x.Id == new UserId(request.UserId))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (user == null)
                throw new NotFoundException(request.UserId);

            var userRoles = user.Roles.Global.ToList().Concat(user.Roles.ByProject.Values.SelectMany(x => x))
                .Distinct().ToList();

            var roles = await _context.GetCollection<Role>().Find(x => userRoles.Contains(x.Id))
                .ToListAsync(cancellationToken);
            var rolesPermissions = roles.ToDictionary(x => x.Id, x => x.Permissions);

            return new UserPermissionsView
            {
                Global = user.Roles.Global.SelectMany(x => rolesPermissions[x]).Select(x => x.Value).Distinct(),
                ByProjects = user.Roles.ByProject.ToDictionary(
                    x => x.Key.Value,
                    x => x.Value.SelectMany(x => rolesPermissions[x]).Select(x => x.Value).Distinct()
                ),
                Custom = user.Permissions.Select(x => x.Value)
            };
        }
    }
}
