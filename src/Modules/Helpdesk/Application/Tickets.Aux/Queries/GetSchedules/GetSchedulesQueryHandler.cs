using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetSchedules
{
    internal class GetSchedulesQueryHandler :
        IQueryHandler<GetSchedulesQuery, IEnumerable<TicketSchedule>>,
        IQueryHandler<GetSchedulesByTicketQuery, IEnumerable<TicketSchedule>>
    {
        private readonly IMongoContext _context;

        public GetSchedulesQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketSchedule>> Handle(GetSchedulesQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<TicketSchedule>().Find(x => request.Statuses.Contains(x.Status))
                .ToListAsync(cancellationToken);
            return result;
        }

        public async Task<IEnumerable<TicketSchedule>> Handle(GetSchedulesByTicketQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<TicketSchedule>().Find(x => x.TicketId == request.TicketId)
                .ToListAsync(cancellationToken);
            return result;
        }
    }
}
