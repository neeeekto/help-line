using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Queries;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;
using HelpLine.Modules.UserAccess.Domain.Users;
using IdentityServer4.Extensions;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers
{
    internal class GetUsersQueryHandler : IQueryHandler<GetUserQuery, UserView>,
        IQueryHandler<GetUsersQuery, IEnumerable<UserView>>
    {
        private readonly IMongoContext _mongoContext;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IMongoContext mongoContext, IMapper mapper)
        {
            _mongoContext = mongoContext;
            _mapper = mapper;
        }

        public async Task<UserView> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _mongoContext.GetCollection<User>().Find(x => x.Id == new UserId(request.UserId))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (user == null)
                throw new NotFoundException(request.UserId);
            return _mapper.Map<UserView>(user);
        }

        public async Task<IEnumerable<UserView>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var skipProjectSearch = request.ProjectId?.IsNullOrEmpty() ?? true;
            var fb = new FilterDefinitionBuilder<User>();
            var filter = fb.Empty;
            if (!skipProjectSearch)
                filter &= fb.Where(x => x.Projects.Contains(new ProjectId(request.ProjectId!)));
            var users = await _mongoContext.GetCollection<User>().Find(filter).ToListAsync(cancellationToken);
            return users.Select(user => _mapper.Map<UserView>(user));
        }
    }
}
