using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Operators.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperators
{
    internal class GetOperatorsQueryHandler : IQueryHandler<GetOperatorsQuery, IEnumerable<OperatorView>>
    {
        private readonly IMongoContext _context;

        public GetOperatorsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OperatorView>> Handle(GetOperatorsQuery request,
            CancellationToken cancellationToken)
        {
            var operators = await _context.GetCollection<Operator>()
                .Find(x => request.OperatorsIds == null || request.OperatorsIds.Contains(x.Id.Value))
                .ToListAsync(cancellationToken);
            return operators.Select(oper => new OperatorView
            {
                Id = oper.Id.Value,
                Favorite = oper.Favorite.Select(x => x.Value)
            });
        }
    }
}
