using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Configuration.Projections;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing
{
    public class ProjectionRunnerCommandHandlerDecorator<T, TResult> : IRequestHandler<T, TResult> where T:IRequest<TResult>
    {
        private readonly IRequestHandler<T, TResult> _decorated;
        private readonly IDomainEventsAccessor _domainEventsAccessor;
        private readonly IEnumerable<IProjector> _projectors;

        public ProjectionRunnerCommandHandlerDecorator(IRequestHandler<T, TResult> decorated,
            IDomainEventsAccessor domainEventsAccessor, IEnumerable<IProjector> projectors)
        {
            _decorated = decorated;
            _domainEventsAccessor = domainEventsAccessor;
            _projectors = projectors;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var result = await _decorated.Handle(command, cancellationToken);
            var events = _domainEventsAccessor.GetAllDomainEvents();

            foreach (var projector in _projectors)
                await projector.Project(events);


            return result;
        }
    }
}
