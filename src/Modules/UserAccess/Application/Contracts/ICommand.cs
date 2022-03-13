using System;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Contracts
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }

    public interface ICommand: IRequest
    {
        Guid Id { get; }
    }
}