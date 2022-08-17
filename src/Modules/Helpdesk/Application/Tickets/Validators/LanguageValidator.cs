using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Validators
{
    public static class LanguageValidation
    {
        public static async Task<bool> Check(IMongoContext ctx, string project, string language, CancellationToken ct)
        {
            var result = await ctx.GetCollection<Project>()
                .Find(x => x.Id == new ProjectId(project) && x.Languages.Any(x => x == new LanguageCode(language)))
                .AnyAsync(ct);
            return result;
        }
    }
}
