using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveBan
{
    internal class RemoveBanCommandHandler : ICommandHandler<RemoveBanCommand>
    {
        private readonly IRepository<Ban> _repository;

        public RemoveBanCommandHandler(IRepository<Ban> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveBanCommand request, CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.BanId);
            return Unit.Value;
        }
    }
}
