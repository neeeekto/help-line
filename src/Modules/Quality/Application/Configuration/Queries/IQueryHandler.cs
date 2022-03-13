using HelpLine.Modules.Quality.Application.Contracts;
using MediatR;

namespace HelpLine.Modules.Quality.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> : 
        IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {

    }
}