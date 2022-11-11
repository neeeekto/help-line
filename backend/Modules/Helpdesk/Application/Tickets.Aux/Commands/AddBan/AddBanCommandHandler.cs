using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.AddBan
{
    internal class AddBanCommandHandler : ICommandHandler<AddBanCommand, Guid>
    {
        private readonly IRepository<Ban> _repository;

        public AddBanCommandHandler(IRepository<Ban> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(AddBanCommand request, CancellationToken cancellationToken)
        {
            var ban = new Ban
            {
                Id = Guid.NewGuid(),
                Value = request.Value,
                Parameter = request.Parameter,
                ExpiredAt = request.ExpiredAt,
                ProjectId = request.ProjectId
            };
            await _repository.Add(ban);
            return ban.Id;
        }
    }
}
