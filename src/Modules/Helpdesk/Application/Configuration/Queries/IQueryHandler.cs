using HelpLine.Modules.Helpdesk.Application.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {

    }
}
