using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetChannelSettings;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetChannelSettings
{
    class GetChannelSettingsQueryHandler : IQueryHandler<GetChannelSettingsQuery, IEnumerable<ChannelSettings>>
    {
        private IMongoContext _context;

        public GetChannelSettingsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChannelSettings>> Handle(GetChannelSettingsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<ChannelSettings>().Find(x => true).ToListAsync(cancellationToken);
            return result;
        }
    }
}
