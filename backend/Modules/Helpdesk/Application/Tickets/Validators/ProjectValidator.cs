using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Validators
{
    public static class ProjectValidator
    {
        public static Func<string, CancellationToken, Task<bool>> Make(IMongoContext ctx) => async (project, ct) =>
        {
            return await ctx.GetCollection<Project>().Find(x => x.Id == new ProjectId(project)).AnyAsync(ct);
        };
    }
}
