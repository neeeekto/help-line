using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.Unsubscribe
{
    class UnsubscribeCommandValidator : AbstractValidator<UnsubscribeCommand>
    {
        public UnsubscribeCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotEmpty().NotNull().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.UserId).NotEmpty().NotNull();
        }
    }
}
