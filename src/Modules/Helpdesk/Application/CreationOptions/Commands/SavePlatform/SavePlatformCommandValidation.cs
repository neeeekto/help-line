using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform
{
    internal class SavePlatformCommandValidation : AbstractValidator<SavePlatformCommand>
    {
        public SavePlatformCommandValidation(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotEmpty().NotNull().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Key).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        }
    }
}
