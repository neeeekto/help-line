using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ReScheduleTimer
{
    internal class ReScheduleTimerCommandHandler : ICommandHandler<ReScheduleTimerCommand>
    {
        private readonly IMongoContext _context;

        public ReScheduleTimerCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ReScheduleTimerCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _context.GetCollection<TicketSchedule>().Find(x => x.Id == request.ScheduleId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (schedule == null)
                throw new NotFoundException(request.ScheduleId);
            if (schedule.Status is TicketSchedule.Statuses.Planned or TicketSchedule.Statuses.InQueue)
                throw new ApplicationException("ReScheduling of Planned or InQueue is not available!");
            await _context.GetCollection<TicketSchedule>().UpdateOneAsync(x => x.Id == request.ScheduleId,
                new UpdateDefinitionBuilder<TicketSchedule>().Set(x => x.Status, TicketSchedule.Statuses.Planned),
                cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
