using FluentValidation;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserSetup
{
    class SetUserSetupCommandValidator : AbstractValidator<SetUserSetupCommand>
    {
        public SetUserSetupCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Setup).NotEmpty();
        }
    }
}