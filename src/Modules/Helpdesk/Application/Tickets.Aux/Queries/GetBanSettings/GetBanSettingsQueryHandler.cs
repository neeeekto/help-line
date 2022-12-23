using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBanSettings
{
    internal class GetBanSettingsQueryHandler : IQueryHandler<GetBanSettingsQuery, BanSettings>
    {
        private readonly IMongoContext _context;

        public GetBanSettingsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public Task<BanSettings> Handle(GetBanSettingsQuery request, CancellationToken cancellationToken)
        {
            return _context.GetCollection<BanSettings>().Find(x => x.ProjectId == request.ProjectId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
