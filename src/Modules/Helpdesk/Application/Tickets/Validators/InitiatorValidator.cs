using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Validators
{
    internal static class InitiatorValidator
    {
        internal class OperatorValidator : AbstractValidator<OperatorInitiatorDto>
        {
            public OperatorValidator(IMongoContext ctx)
            {
                RuleFor(x => x.OperatorId).NotNull().MustAsync(((id, ct) =>
                        ctx.GetCollection<Operator>().Find(x => x.Id == new OperatorId(id)).AnyAsync(ct)))
                    .WithMessage("Operator not exist");
            }
        }

        internal class SystemInitiatorValidator : AbstractValidator<SystemInitiatorDto>
        {
            public SystemInitiatorValidator(IMongoContext ctx)
            {
            }
        }

        internal class UserInitiatorValidator : AbstractValidator<UserInitiatorDto>
        {
            public UserInitiatorValidator(IMongoContext ctx)
            {
                RuleFor(x => x.UserId).NotNull().NotEmpty();
            }
        }
    }
}
