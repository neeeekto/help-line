using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Operators.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.SetOperatorRoles
{
    internal class SetOperatorRolesCommandHandler : ICommandHandler<SetOperatorRolesCommand>
    {
        private readonly IOperatorRepository _operatorRepository;

        public SetOperatorRolesCommandHandler(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        public async Task<Unit> Handle(SetOperatorRolesCommand request, CancellationToken cancellationToken)
        {
            var oper = await _operatorRepository.Get(new OperatorId(request.OperatorId));
            if (oper == null)
            {
                throw new NotFoundException(request.OperatorId);
            }

            oper.SetRole(request.ProjectId, request.RoleIds);
            await _operatorRepository.Update(oper);
            return Unit.Value;
        }
    }
}
