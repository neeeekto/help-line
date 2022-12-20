using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketReopenCondition
{
    internal class SaveTicketReopenConditionCommandHandler : ICommandHandler<SaveTicketReopenConditionCommand, string>
    {
        private readonly IRepository<TicketReopenCondition> _repository;

        public SaveTicketReopenConditionCommandHandler(IRepository<TicketReopenCondition> repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(SaveTicketReopenConditionCommand request,
            CancellationToken cancellationToken)
        {
            var condition = new TicketReopenCondition
            {
                Enabled = false,
                Name = request.Data.Name,
                MinimalScore = request.Data.MinimalScore,
                MustSolved = request.Data.MustSolved,
                ProjectId = request.ProjectId
            };
            await _repository.Update(condition, true);
            return condition.Id;
        }
    }
}
