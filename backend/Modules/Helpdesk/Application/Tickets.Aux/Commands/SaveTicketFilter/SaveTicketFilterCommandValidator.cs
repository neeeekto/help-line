using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketFilter
{
    internal class SaveTicketFilterCommandValidator : AbstractValidator<SaveTicketFilterCommand>
    {
        public SaveTicketFilterCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotEmpty().NotNull().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Data).NotNull().SetValidator(new TicketFilterDataValidator(ctx));
        }
    }
}
