using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators
{
    internal class TicketFilterDataValidator : AbstractValidator<TicketFilterData>
    {
        public TicketFilterDataValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Filter).NotNull();
            RuleFor(x => x.Features).NotNull();
            RuleFor(x => x.Share).SetInheritanceValidator(v =>
            {
                v.Add(new TicketFilterShareForOperatorsValidator(ctx));
            });
        }

        class TicketFilterShareForOperatorsValidator : AbstractValidator<TicketFilterShareForOperators>
        {
            public TicketFilterShareForOperatorsValidator(IMongoContext ctx)
            {
                RuleFor(x => x.Operators).NotNull().MustAsync(OperatorsValidator.Make(ctx));
            }
        }
    }
}
