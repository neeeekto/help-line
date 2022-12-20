using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators
{
    internal class BanSettingsValidator : AbstractValidator<BanSettings>
    {
        public BanSettingsValidator()
        {
            RuleFor(x => x.Interval).NotNull().Must(x => x.Ticks > 0);
            RuleFor(x => x.BanDelay).NotNull().Must(x => x.Ticks > 0);
            RuleFor(x => x.TicketsCount).GreaterThan(0);
        }
    }
}
