using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using MongoDB.Driver;
using Tag = HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Tag;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Validators
{
    public static class TagValidation
    {
        public static async Task<bool> Check(IMongoContext ctx, string tag, string projectId)
        {
            return await ctx.GetCollection<Tag>().Find(x => x.Key == tag && x.ProjectId == projectId).AnyAsync();
        }

        public static async Task<bool> Check(IMongoContext ctx, IEnumerable<string> tags, string projectId)
        {
            var foundTags = await ctx.GetCollection<Tag>().Find(x => tags.Contains(x.Key) && x.ProjectId == projectId)
                .CountDocumentsAsync();
            return foundTags == tags.Count();
        }
    }
}
