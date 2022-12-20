using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Projects.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Commands.UpdateProject
{
    internal class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.Data).NotNull().SetValidator(new ProjectDataValidator());
        }
    }
}
