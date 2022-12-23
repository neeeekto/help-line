using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Validations;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.SaveAutoreplyScenario
{
    internal class SaveAutoreplyScenarioCommandValidator : AbstractValidator<SaveAutoreplyScenarioCommand>
    {
        public SaveAutoreplyScenarioCommandValidator(IMongoContext ctx)
        {
            RuleFor(x => x.Scenario).NotNull().SetValidator(new AutoreplyScenarioValidator(ctx));
        }
    }
}
