using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Jobs;
using HelpLine.Modules.Helpdesk.Jobs;
using MediatR;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Jobs
{
    internal class CollectEmailMessagesJobHandler : IJobHandler<CollectEmailMessagesJob>
    {
        private readonly ILogger _logger;

        public CollectEmailMessagesJobHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(CollectEmailMessagesJob request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
