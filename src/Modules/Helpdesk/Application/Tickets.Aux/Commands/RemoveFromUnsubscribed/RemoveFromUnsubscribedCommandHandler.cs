using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveFromUnsubscribed
{
    internal class RemoveFromUnsubscribedCommandHandler : ICommandHandler<RemoveFromUnsubscribedCommand>
    {
        private readonly IRepository<Unsubscribe> _repository;

        public RemoveFromUnsubscribedCommandHandler(IRepository<Unsubscribe> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveFromUnsubscribedCommand request, CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.UnsubscribeId);
            return Unit.Value;
        }
    }
}
