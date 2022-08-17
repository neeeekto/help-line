using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Operators.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateOperator
{
    internal class CreateOperatorCommandHandler : ICommandHandler<CreateOperatorCommand>
    {
        private readonly IOperatorRepository _operatorRepository;

        public CreateOperatorCommandHandler(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        public async Task<Unit> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
        {
            var oper = await Operator.Create(new OperatorId(request.OperatorId));
            await _operatorRepository.Add(oper);
            return Unit.Value;
        }
    }
}
