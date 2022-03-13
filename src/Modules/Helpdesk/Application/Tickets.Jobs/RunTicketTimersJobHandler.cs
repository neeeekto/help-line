using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Configuration.Jobs;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Jobs.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Jobs.Commands.ExecuteTicketSchedule;
using HelpLine.Modules.Helpdesk.Jobs;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Jobs
{
    internal class RunTicketTimersJobHandler : IJobHandler<RunTicketTimersJob>
    {
        private readonly IMongoContext _context;
        private readonly ICommandsScheduler _scheduler;

        public RunTicketTimersJobHandler(IMongoContext context, ICommandsScheduler scheduler)
        {
            _context = context;
            _scheduler = scheduler;
        }

        public async Task<Unit> Handle(RunTicketTimersJob request, CancellationToken cancellationToken)
        {
            var schedules = await _context.GetCollection<TicketSchedule>()
                .Find(x => x.TriggerDate <= DateTime.UtcNow && x.Status == TicketSchedule.Statuses.Planned)
                .Limit(500)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (var schedule in schedules)
            {

                schedule.Status = TicketSchedule.Statuses.InQueue;
                // Immediately, because we can't wait session
                await _context.GetCollection<TicketSchedule>().ReplaceOneAsync(x => x.Id == schedule.Id,
                    schedule, cancellationToken: cancellationToken);
                await _scheduler.EnqueueAsync(new ExecuteTicketScheduleCommand(schedule.Id, schedule.TicketId,
                    schedule.Id));
            }

            return Unit.Value;
        }
    }
}
