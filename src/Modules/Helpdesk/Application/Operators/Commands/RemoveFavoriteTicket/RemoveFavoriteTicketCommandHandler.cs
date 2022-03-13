using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Operators.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.RemoveFavoriteTicket
{
    internal class RemoveFavoriteTicketCommandHandler : ICommandHandler<RemoveFavoriteTicketCommand>
    {
        private readonly IOperatorRepository _operatorRepository;

        public RemoveFavoriteTicketCommandHandler(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        public async Task<Unit> Handle(RemoveFavoriteTicketCommand request, CancellationToken cancellationToken)
        {
            var oper = await _operatorRepository.Get(new OperatorId(request.OperatorId));
            if (oper == null)
                throw new NotFoundException(request.OperatorId);
            oper.RemoveFromFavorite(new TicketId(request.TicketId));
            await _operatorRepository.Update(oper);
            return Unit.Value;
        }
    }
}
