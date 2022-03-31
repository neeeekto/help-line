using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketFilter
{
    internal class SaveTicketFilterCommandHandler : ICommandHandler<SaveTicketFilterCommand, Guid>
    {
        private readonly IRepository<TicketFilter> _repository;
        private readonly IExecutionContextAccessor _accessor;

        public SaveTicketFilterCommandHandler(IRepository<TicketFilter> repository, IExecutionContextAccessor accessor)
        {
            _repository = repository;
            _accessor = accessor;
        }

        public async Task<Guid> Handle(SaveTicketFilterCommand request, CancellationToken cancellationToken)
        {
            var filter = new TicketFilter
            {
                Changed = DateTime.UtcNow,
                Owner = _accessor.IsAvailable ? _accessor.UserId : null,
                Features = request.Data.Features,
                Filter = request.Data.Filter,
                Id = request.FilterId ?? Guid.NewGuid(),
                Name = request.Data.Name,
                ProjectId = request.ProjectId,
                Share = request.Data.Share
            };
            await _repository.Update(filter, true);
            return filter.Id;
        }
    }
}
