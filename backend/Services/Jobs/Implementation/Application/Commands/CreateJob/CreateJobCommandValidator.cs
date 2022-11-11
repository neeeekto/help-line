using System.Linq;
using FluentValidation;
using HelpLine.Services.Jobs.Infrastructure;
using MongoDB.Driver;

namespace HelpLine.Services.Jobs.Application.Commands.CreateJob
{
    internal class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator(JobsMongoContext context, JobTasksCollection tasksCollection)
        {
            RuleFor(x => x.Data.Name).NotEmpty().NotNull().MustAsync(async (name, ct) =>
                !await context.Jobs.Find(x => x.Name.ToLower() == name.ToLowerInvariant()).AnyAsync(ct));
            RuleFor(x => x.Data.Schedule).NotEmpty().NotNull();
            RuleFor(x => x.Task)
                .NotEmpty().NotNull()
                .Must(name => tasksCollection.Tasks.Any(x => x.Name == name));
        }
    }
}
