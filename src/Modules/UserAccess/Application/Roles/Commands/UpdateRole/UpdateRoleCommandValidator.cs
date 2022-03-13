using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Domain.Roles;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.UpdateRole
{
    class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator(IMongoContext mongoContext)
        {
            RuleFor(x => x.Permissions).NotEmpty().WithMessage("Permissions can not be empty");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can not be empty")
                .MustAsync(async (command, ctx, ct) =>
                    await mongoContext.GetCollection<Role>()
                        .Find(x => x.Id != new RoleId(command.RoleId) && x.Name == command.Name)
                        .CountDocumentsAsync() == 0
                ).WithMessage("Name can be unique");
        }
    }
}