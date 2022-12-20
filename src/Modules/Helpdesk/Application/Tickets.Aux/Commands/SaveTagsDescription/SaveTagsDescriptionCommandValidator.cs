using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Validators;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTagsDescription
{
    internal class SaveTagsDescriptionCommandValidator : AbstractValidator<SaveTagsDescriptionCommand>
    {
        public SaveTagsDescriptionCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Tag).NotNull().NotEmpty()
                .MustAsync((command, tag, ct) => TagValidation.Check(ctx, tag, command.ProjectId));
            RuleFor(x => x.ProjectId).NotNull()
                .NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Issues).NotNull()
                .ForEach(x => x.SetValidator(new TagsDescriptionIssueValidator()));
        }
    }
}
