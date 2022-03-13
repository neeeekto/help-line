using System;
using MediatR;

namespace HelpLine.Modules.Quality.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }
}