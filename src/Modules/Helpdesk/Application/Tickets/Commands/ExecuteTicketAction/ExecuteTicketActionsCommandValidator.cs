using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction
{
    class ExecuteTicketActionsCommandValidator : AbstractValidator<ExecuteTicketActionsCommand>
    {
        public ExecuteTicketActionsCommandValidator(IMongoContext ctx)
        {
            RuleFor(x => x.TicketId).NotNull().NotEmpty();
            RuleFor(x => x.Initiator).NotNull().SetInheritanceValidator(v =>
            {
                v.Add(new InitiatorValidator.OperatorValidator(ctx));
                v.Add(new InitiatorValidator.SystemInitiatorValidator(ctx));
                v.Add(new InitiatorValidator.UserInitiatorValidator(ctx));
            });
        }
    }
}
