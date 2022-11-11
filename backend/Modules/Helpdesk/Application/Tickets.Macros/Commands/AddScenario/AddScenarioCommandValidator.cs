using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.AddScenario
{
    class AddScenarioCommandValidator : AbstractValidator<AddScenarioCommand>
    {
        public AddScenarioCommandValidator(IMongoContext ctx)
        {
            var scenarios = ctx.GetCollection<Scenario>();
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(async (name, cancellationToken) =>
                {
                    var exist = await scenarios.Find(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant()).AnyAsync();
                    return !exist;
                }).WithMessage("Name already exist");
            RuleFor(x => x.Actions).NotEmpty();
            RuleFor(x => x.Triggers).NotEmpty();
        }
    }
}
