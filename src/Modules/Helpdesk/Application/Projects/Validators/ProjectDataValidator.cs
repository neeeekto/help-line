using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Validators
{
    internal class ProjectDataValidator : AbstractValidator<ProjectDataDto>
    {
        public ProjectDataValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Languages).NotEmpty().ForEach(x => x.NotEmpty().NotNull());
        }
    }
}
