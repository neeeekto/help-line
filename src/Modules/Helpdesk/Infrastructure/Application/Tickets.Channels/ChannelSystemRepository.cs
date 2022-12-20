using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Channels
{
    internal class ChannelSystemRepository : RepositoryBase<ChannelSettings>, IChannelSystemRepository
    {
        public ChannelSystemRepository(IMongoContext context) : base(context)
        {
        }

        protected override Expression<Func<ChannelSettings, bool>> GetIdFilter(ChannelSettings entity)
        {
            return x => x.Key == entity.Key && x.ProjectId == entity.ProjectId;
        }

        public async Task<T> Get<T>(string projectId) where T : ChannelSettings
        {
            var fb = new FilterDefinitionBuilder<ChannelSettings>();
            var filter = fb.OfType<T>() & fb.Eq(x => x.ProjectId, projectId);
            var result = await Collection.Find(filter).FirstOrDefaultAsync();
            return result as T;
        }
    }
}
