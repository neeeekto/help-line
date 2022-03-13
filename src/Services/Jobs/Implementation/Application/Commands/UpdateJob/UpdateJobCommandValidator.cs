using System.Linq;
using FluentValidation;
using HelpLine.Services.Jobs.Infrastructure;
using MongoDB.Driver;

namespace HelpLine.Services.Jobs.Application.Commands.UpdateJob
{
    internal class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobCommandValidator(JobsMongoContext context, JobTasksCollection tasksCollection)
        {
            RuleFor(x => x.Data.Name).NotEmpty().NotNull().MustAsync(async (command, value, ct) =>
                !await context.Jobs.Find(x => x.Id != command.JobId && x.Name.ToLower() == value.ToLowerInvariant())
                    .AnyAsync(ct));
            RuleFor(x => x.Data.Schedule).NotEmpty().NotNull();
        }
    }
}
