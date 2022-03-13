using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Domain.Roles;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.CreateRole
{
    class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator(IMongoContext mongoContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name can not be empty")
                .MustAsync(async (name, ct) =>
                    await mongoContext.GetCollection<Role>()
                        .Find(x => x.Name == name)
                        .CountDocumentsAsync() == 0)
                .WithMessage("Name must be unique");
            RuleFor(x => x.Permissions).NotEmpty();
        }
    }
}