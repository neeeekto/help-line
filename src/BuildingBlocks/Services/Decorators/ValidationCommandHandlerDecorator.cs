using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HelpLine.BuildingBlocks.Application;
using MediatR;

namespace HelpLine.BuildingBlocks.Services.Decorators
{
    internal class ValidationCommandHandlerDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        private readonly IList<IValidator<TRequest>> _validators;
        private readonly IRequestHandler<TRequest, TResult> _decorated;

        public ValidationCommandHandlerDecorator(
            IList<IValidator<TRequest>> validators,
            IRequestHandler<TRequest, TResult> decorated)
        {
            _validators = validators;
            _decorated = decorated;
        }

        public async Task<TResult> Handle(TRequest command, CancellationToken cancellationToken)
        {
            var validateTasks = await Task.WhenAll(_validators
                .Select(v => v.ValidateAsync(command, cancellationToken)));

            var errors = validateTasks
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (errors.Any())
                throw new InvalidCommandException(errors.Select(x => x.ErrorMessage));

            return await _decorated.Handle(command, cancellationToken);
        }
    }
}
