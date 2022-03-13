using System.Collections.Generic;
using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Validators
{
    public class ProblemAndThemeContentsValidator : AbstractValidator<KeyValuePair<string, ProblemAndThemeContent>>
    {
        public ProblemAndThemeContentsValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Key).NotEmpty().NotNull();
            RuleFor(x => x.Value).NotEmpty().NotNull().SetValidator(new ProblemAndThemeContentValidator());
        }
    }
}
