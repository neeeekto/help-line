using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Operators.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.AddFavoriteTicket
{
    internal class AddFavoriteTicketCommandHandler : ICommandHandler<AddFavoriteTicketCommand>
    {
        private readonly IOperatorRepository _operatorRepository;

        public AddFavoriteTicketCommandHandler(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        public async Task<Unit> Handle(AddFavoriteTicketCommand request, CancellationToken cancellationToken)
        {
            var oper = await _operatorRepository.Get(new OperatorId(request.OperatorId));
            if (oper == null)
                throw new NotFoundException(request.OperatorId);
            oper.AddToFavorite(new TicketId(request.TicketId));
            await _operatorRepository.Update(oper);
            return Unit.Value;
        }
    }
}
