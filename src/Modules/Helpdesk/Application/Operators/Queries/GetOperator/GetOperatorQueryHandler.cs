using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Operators.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperator
{
    internal class GetOperatorQueryHandler : IQueryHandler<GetOperatorQuery, OperatorView>
    {
        private readonly IMongoContext _context;

        public GetOperatorQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<OperatorView> Handle(GetOperatorQuery request, CancellationToken cancellationToken)
        {
            var oper = await _context.GetCollection<Operator>()
                .Find(x => x.Id == new OperatorId(request.OperatorId)).FirstOrDefaultAsync(cancellationToken);
            if (oper == null)
                throw new NotFoundException(request.OperatorId);
            return new OperatorView
            {
                Id = oper.Id.Value,
                Favorite = oper.Favorite.Select(x => x.Value),
                Roles = oper.Roles
            };
        }
    }
}
