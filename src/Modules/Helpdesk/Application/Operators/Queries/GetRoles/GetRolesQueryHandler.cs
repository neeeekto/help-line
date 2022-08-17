using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetRoles
{
    internal class GetRolesQueryHandler :
        IQueryHandler<GetRolesQuery, IEnumerable<OperatorRole>>,
        IQueryHandler<GetRoleQuery, OperatorRole>
    {
        private readonly IMongoContext _context;

        public GetRolesQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OperatorRole>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return await _context.GetCollection<OperatorRole>().Find(x => true).ToListAsync(cancellationToken);
        }

        public async Task<OperatorRole> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            return await _context.GetCollection<OperatorRole>().Find(x => x.Id == request.OperatorRoleId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
