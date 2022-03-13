using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag
{
    internal class SaveTagCommandValidator : AbstractValidator<SaveTagCommand>
    {
        public SaveTagCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotNull().NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Key).NotNull().NotEmpty();
        }
    }
}
