using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Validations
{
    internal class AutoreplyScenarioValidator : AbstractValidator<AutoreplyScenario>
    {
        public AutoreplyScenarioValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.ProjectId).NotNull().NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Weight).Must(x => x > 0);
            RuleFor(x => x.Condition).NotNull().SetValidator(new AutoreplyScenarioConditionValidation());
            RuleFor(x => x.Action).NotNull().SetValidator(new AutoreplyScenarioActionValidation());
        }
    }
}
