using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Validators
{
    public class ProblemAndThemeContentValidator : AbstractValidator<ProblemAndThemeContent>
    {
        public ProblemAndThemeContentValidator()
        {
            RuleFor(x => x.Notification).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotNull().NotEmpty();
        }
    }
}
