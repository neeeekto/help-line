using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketFilter
{
    internal class RemoveTicketFilterCommandValidator : AbstractValidator<RemoveTicketFilterCommand>
    {
        public RemoveTicketFilterCommandValidator(IMongoContext ctx)
        {
            RuleFor(x => x.FilterId).MustAsync(async (filterId, ct) =>
            {
                var scenarios = await ctx.GetCollection<Scenario>()
                    .CountDocumentsAsync(x => Enumerable.Contains(x.Filters, filterId), cancellationToken: ct);
                return scenarios == 0;
            }).WithMessage("Filter is using in macros");
        }
    }
}
