using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Queries;
using HelpLine.Modules.UserAccess.Application.Roles.ViewsModels;
using HelpLine.Modules.UserAccess.Domain.Roles;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Roles.Queries.GetRoles
{
    class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, IEnumerable<RoleView>>
    {
        private readonly IMongoContext _mongoContext;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IMongoContext mongoContext, IMapper mapper)
        {
            _mongoContext = mongoContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleView>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _mongoContext.GetCollection<Role>().Find(x => true)
                .ToListAsync(cancellationToken: cancellationToken);
            return _mapper.Map<IEnumerable<RoleView>>(roles);
        }
    }
}
