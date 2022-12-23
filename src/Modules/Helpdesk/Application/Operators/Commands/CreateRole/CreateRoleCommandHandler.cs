using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole
{
    internal class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Guid>
    {
        private readonly IRepository<OperatorRole> _repository;

        public CreateRoleCommandHandler(IRepository<OperatorRole> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            await _repository.Add(new OperatorRole()
            {
                Id = id,
                Data = request.RoleData
            });
            return id;
        }
    }
}
