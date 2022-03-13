using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Jobs;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Notifications;
using HelpLine.Modules.Helpdesk.Jobs;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Jobs
{
    internal class CheckMacrosOnScheduleJobHandler : IJobHandler<CheckMacrosOnScheduleJob>
    {
        private readonly IMongoContext _context;
        private readonly IMediator _mediator;
        private IMongoCollection<ScenarioSchedule> Collection => _context.GetCollection<ScenarioSchedule>();

        public CheckMacrosOnScheduleJobHandler(IMongoContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckMacrosOnScheduleJob request, CancellationToken cancellationToken)
        {
            var readyForRun = await Collection.Find(x => x.NextTriggerDate <= DateTime.UtcNow)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (var scenarioSchedule in readyForRun)
                await _mediator.Publish(
                    new TriggerScheduleFireNotification(scenarioSchedule.Interval, scenarioSchedule.Scenarios),
                    cancellationToken);
            return Unit.Value;
        }
    }
}
