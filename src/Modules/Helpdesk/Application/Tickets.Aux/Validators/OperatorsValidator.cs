using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators
{
    internal static class OperatorsValidator
    {
        public static Func<IEnumerable<Guid>?, CancellationToken, Task<bool>> Make(IMongoContext ctx) =>
            async (operators, ct) =>
            {
                if (operators == null || !operators.Any())
                    return true;
                var ids = operators.Select(x => new OperatorId(x));
                var exist = await ctx.GetCollection<Operator>()
                    .CountDocumentsAsync(x => ids.Contains(x.Id), cancellationToken: ct);
                return exist == operators.Count();
            };

        public static Func<Guid, CancellationToken, Task<bool>> MakeForOne(IMongoContext ctx) =>
            (oper, ct) => Make(ctx)(new[] {oper}, ct);
    }
}
