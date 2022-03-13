using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.DeleteScheduleTimer
{
    internal class DeleteScheduleTimerCommandHandler : ICommandHandler<DeleteScheduleTimerCommand>
    {
        private readonly IMongoContext _context;

        public DeleteScheduleTimerCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteScheduleTimerCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _context.GetCollection<TicketSchedule>().Find(x => x.Id == request.ScheduleId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (schedule == null)
                return Unit.Value;
            if (schedule.Status is TicketSchedule.Statuses.Planned or TicketSchedule.Statuses.InQueue)
                throw new ApplicationException($"You cannot delete a schedule with the status {schedule.Status.ToString()}");
            await _context.GetCollection<TicketSchedule>()
                .DeleteOneAsync(x => x.Id == request.ScheduleId, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
