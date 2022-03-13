using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Operators.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.RoleData).SetValidator(new OperatorRoleDataValidator());
        }
    }
}
