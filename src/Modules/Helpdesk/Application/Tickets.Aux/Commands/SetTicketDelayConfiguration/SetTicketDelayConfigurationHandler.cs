using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetTicketDelayConfiguration
{
    class SetTicketDelayConfigurationHandler : ICommandHandler<SetTicketDelayConfigurationCommand>
    {
        private readonly IRepository<TicketsDelayConfiguration> _repository;

        public SetTicketDelayConfigurationHandler(IRepository<TicketsDelayConfiguration> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(SetTicketDelayConfigurationCommand request, CancellationToken cancellationToken)
        {
            var config = new TicketsDelayConfiguration
            {
                ProjectId = request.ProjectId,
                InactivityDelay = request.InactivityDelay,
                FeedbackCompleteDelay = request.FeedbackCompleteDelay,
                LifeCycleDelay = request.LifeCycleDelay
            };
            await _repository.Update(config, true);
            return Unit.Value;
        }
    }
}
