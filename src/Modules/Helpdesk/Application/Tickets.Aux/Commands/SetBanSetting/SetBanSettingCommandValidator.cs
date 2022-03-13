using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Validators;
using HelpLine.Modules.Helpdesk.Application.Tickets.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetBanSetting
{
    internal class SetBanSettingCommandValidator : AbstractValidator<SetBanSettingCommand>
    {
        public SetBanSettingCommandValidator(IMongoContext ctx)
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.ProjectId).NotNull().NotEmpty().MustAsync(ProjectValidator.Make(ctx));
            RuleFor(x => x.Settings).NotNull().SetValidator(new BanSettingsValidator());
        }
    }
}
