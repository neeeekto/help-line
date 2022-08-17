using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.UpdateScenario
{
    class UpdateScenarioCommandValidator : AbstractValidator<UpdateScenarioCommand>
    {
        public UpdateScenarioCommandValidator(IMongoContext ctx)
        {
            var scenarios = ctx.GetCollection<Scenario>();
            RuleFor(x => x)
                .MustAsync(async (cmd, cancellationToken) =>
                {
                    var exist = await scenarios.Find(x =>
                        x.Name.ToLowerInvariant() == cmd.Name.ToLowerInvariant() && x.Id == cmd.ScenarioId).AnyAsync();
                    return !exist;
                }).WithMessage("Name already exist");
            RuleFor(x => x.ScenarioId).NotEmpty();
            RuleFor(x => x.Actions).NotEmpty();
            RuleFor(x => x.Triggers).NotEmpty();
        }
    }
}
