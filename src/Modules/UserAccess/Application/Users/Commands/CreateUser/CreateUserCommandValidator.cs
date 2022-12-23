using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Domain.Roles;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser
{
    class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IMongoContext _mongoContext;

        public CreateUserCommandValidator(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email adress is invalid");

            RuleFor(x => x.Info.Language).NotEmpty().WithMessage("User language can not be empty");
            RuleFor(x => x.Info.FirstName).NotEmpty().WithMessage("User first name can not be empty");
            RuleFor(x => x.Info.LastName).NotEmpty().WithMessage("User last name can not be empty");
            RuleFor(x => x.Permissions).NotNull().ForEach(x => x.NotNull().NotEmpty());
            RuleFor(x => x.GlobalRoles)
                .CustomAsync(CheckRoles);
            RuleFor(x => x.ProjectsRoles).CustomAsync((dictionary, context, cancellationToken) =>
                CheckRoles(dictionary.SelectMany(x => x.Value), context, cancellationToken));
        }

        private async Task CheckRoles(IEnumerable<Guid> roles, ValidationContext<CreateUserCommand> context,
            CancellationToken cancellationToken)
        {
            var roleIds = roles.Select(x => new RoleId(x));
            var foudRoles = await _mongoContext.GetCollection<Role>().Find(x => roleIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
            var notFound = roleIds.Except(foudRoles.Select(x => x.Id));
            if (notFound.Any())
                context.AddFailure(new ValidationFailure(context.PropertyName,
                    $"Roles {string.Join(",", notFound)} not found"));
        }
    }
}