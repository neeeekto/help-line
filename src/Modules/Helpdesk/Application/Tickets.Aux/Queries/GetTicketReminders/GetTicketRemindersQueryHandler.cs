using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReminders
{
    internal class GetTicketRemindersQueryHandler : IQueryHandler<GetTicketRemindersQuery, IEnumerable<TicketReminderEntity>>
    {
        private readonly IMongoContext _ctx;

        public GetTicketRemindersQueryHandler(IMongoContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<TicketReminderEntity>> Handle(GetTicketRemindersQuery request,
            CancellationToken cancellationToken)
        {
            return await _ctx.GetCollection<TicketReminderEntity>().Find(x => x.ProjectId == request.ProjectId &&
                                                                              (request.Enabled == null || x.Enabled == request.Enabled))
                .ToListAsync(cancellationToken);
        }
    }
}
