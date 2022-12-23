using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Validators;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SaveProblemAndTheme
{
    internal class SaveProblemAndThemeCommandValidator : AbstractValidator<SaveProblemAndThemeCommand>
    {
        public SaveProblemAndThemeCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Tag).NotEmpty().MustAsync(
                (cmd, tag, ct) => TagValidation.Check(ctx, tag, cmd.ProjectId));
            RuleFor(x => x.Entity).NotEmpty().SetValidator((cmd, val) =>
                new ProblemAndThemeValidator(ctx, cmd.ProjectId, new string[] { }, new string[] {}, false));
        }
    }
}
