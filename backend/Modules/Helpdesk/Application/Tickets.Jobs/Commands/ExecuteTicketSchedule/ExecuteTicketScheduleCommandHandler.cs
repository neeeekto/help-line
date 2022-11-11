using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Jobs.Commands.NotifyAboutDeadTimer;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Jobs.Commands.ExecuteTicketSchedule
{
    class ExecuteTicketScheduleCommandHandler : ICommandHandler<ExecuteTicketScheduleCommand>
    {
        private readonly IMongoContext _context;
        private readonly ITicketServicesProvider _ticketServicesProvider;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly ICommandsScheduler _scheduler;

        public ExecuteTicketScheduleCommandHandler(IMongoContext context,
            ITicketServicesProvider ticketServicesProvider, ITicketsRepository ticketsRepository,
            ICommandsScheduler scheduler)
        {
            _context = context;
            _ticketServicesProvider = ticketServicesProvider;
            _ticketsRepository = ticketsRepository;
            _scheduler = scheduler;
        }


        public async Task<Unit> Handle(ExecuteTicketScheduleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ticket =
                    await _ticketsRepository.GetByIdAsync(new TicketId(request.TicketId), cancellationToken);
                if (ticket == null) throw new NotFoundException(request.TicketId);

                var isHandler = await ticket.Execute(new RunTicketScheduleCommand(new ScheduleId(request.ScheduleId)),
                    _ticketServicesProvider,
                    new SystemInitiator());

                await _ticketsRepository.SaveAsync(ticket, cancellationToken);
                if (!isHandler)
                {
                    await _context.GetCollection<TicketSchedule>().UpdateOneAsync(
                        _context.Session,
                        x => x.Id == request.Id,
                        new UpdateDefinitionBuilder<TicketSchedule>()
                            .Set(x => x.Status, TicketSchedule.Statuses.Dead),
                        cancellationToken: cancellationToken);
                    await _scheduler.EnqueueAsync(new NotifyAboutDeadTicketScheduleCommand(request.ScheduleId));
                }
                // We CAN'T remove TicketSchedule from here! Ticket will delete it itself
            }
            catch (Exception e)
            {
                var schedule = await _context.GetCollection<TicketSchedule>().Find(x => x.Id == request.ScheduleId)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (schedule != null)
                {
                    schedule.Status = TicketSchedule.Statuses.Problem;
                    schedule.Details = $"{e.Message}\n {e.StackTrace}";
                    await _context.GetCollection<TicketSchedule>().ReplaceOneAsync(_context.Session,
                        x => x.Id == schedule.Id,
                        schedule, cancellationToken: cancellationToken);
                }
            }

            return Unit.Value;
        }
    }
}
