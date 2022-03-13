using System.Linq;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetTicketDelayConfiguration
{
    class SetTicketDelayConfigurationValidator : AbstractValidator<SetTicketDelayConfigurationCommand>
    {
        public SetTicketDelayConfigurationValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotEmpty().NotNull().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.InactivityDelay).NotEmpty().NotNull().Must(x => x.Ticks > 0);
            RuleFor(x => x.FeedbackCompleteDelay).NotEmpty().NotNull().Must(x => x.Ticks > 0);
            RuleFor(x => x.LifeCycleDelay).NotNull().NotEmpty()
                .Must(x => x.ContainsKey(TicketLifeCycleType.Closing))
                .Must(x => x.ContainsKey(TicketLifeCycleType.Feedback))
                .Must(x => x.ContainsKey(TicketLifeCycleType.Resolving))
                .Must(x => x.All(x => x.Value.Ticks > 0));
        }
    }
}
