using System.Linq;
using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Validations
{
    internal class TagConditionValidation : AbstractValidator<TagCondition>
    {
        public TagConditionValidation()
        {
            RuleFor(x => x.Tags).NotNull().NotEmpty().Must(x => x.Any());
        }
    }
}
