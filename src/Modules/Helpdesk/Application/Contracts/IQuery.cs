using System;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }
}
