using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketMessageTemplates
{
    internal class GetTicketMessageTemplatesQueryHandler : IQueryHandler<GetTicketMessageTemplatesQuery, IEnumerable<TicketMessageTemplate>>
    {
        private readonly IMongoContext _ctx;

        public GetTicketMessageTemplatesQueryHandler(IMongoContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<TicketMessageTemplate>> Handle(GetTicketMessageTemplatesQuery request,
            CancellationToken cancellationToken)
        {
            return await _ctx.GetCollection<TicketMessageTemplate>().Find(x => x.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
