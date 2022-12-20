using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Jobs
{
    public class UnitOfWorkJobHandler<T, TResult> : IRequestHandler<T, TResult> where T : IRequest<TResult>
    {
        private readonly IRequestHandler<T, TResult> _decorated;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkJobHandler(IRequestHandler<T, TResult> decorated, IUnitOfWork unitOfWork)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            // Nested commands
            if (_unitOfWork.Transaction)
            {
                var result = await _decorated.Handle(command, cancellationToken);
                return result;
            }

            await _unitOfWork.Begin(cancellationToken);
            try
            {
                var result = await _decorated.Handle(command, cancellationToken);
                await _unitOfWork.Commit(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                await _unitOfWork.Abort(cancellationToken);
                throw;
            }
        }
    }
}
