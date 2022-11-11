using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HelpLine.BuildingBlocks.Application;
using MediatR;

namespace HelpLine.BuildingBlocks.Infrastructure
{
    public class ValidationCommandHandlerDecorator<T, TResult> : IRequestHandler<T, TResult> where T:IRequest<TResult>
    {
        private readonly IList<IValidator<T>> _validators;
        private readonly IRequestHandler<T, TResult> _decorated;

        public ValidationCommandHandlerDecorator(
            IList<IValidator<T>> validators,
            IRequestHandler<T, TResult> decorated)
        {
            _validators = validators;
            _decorated = decorated;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var validateTasks = await Task.WhenAll(_validators
                .Select(v => v.ValidateAsync(command, cancellationToken)));

            var errors = validateTasks
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (errors.Any())
            {
                throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
            }

            return await _decorated.Handle(command, cancellationToken);
        }
    }
}
