using FluentValidation;
using HelpLine.Modules.Helpdesk.Application.Operators.Validators;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.UpdateRole
{
    internal class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.Data).SetValidator(new OperatorRoleDataValidator());
        }
    }
}
