using System.Linq;
using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Validations
{
    internal class AutoreplyScenarioConditionValidation : AbstractValidator<AutoreplyScenarioCondition>
    {
        public AutoreplyScenarioConditionValidation()
        {
            RuleFor(x => x.Languages).NotNull().NotEmpty().Must(x => x.Any());
            RuleFor(x => x.TagConditions).NotNull()
                .NotEmpty()
                .Must(x => x.Any())
                .ForEach(x => x.SetValidator(new TagConditionValidation()));
        }
    }
}
