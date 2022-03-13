using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketMessageTemplate
{
    internal class RemoveTicketMessageTemplateCommandHandler : ICommandHandler<RemoveTicketMessageTemplateCommand>
    {
        private readonly IRepository<TicketMessageTemplate> _repository;

        public RemoveTicketMessageTemplateCommandHandler(IRepository<TicketMessageTemplate> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveTicketMessageTemplateCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.Remove(x => x.Id == request.TemplateId);
            return Unit.Value;
        }
    }
}
