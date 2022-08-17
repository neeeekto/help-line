using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Services
{
    public class TicketScheduler : ITicketScheduler
    {
        private readonly IMongoContext _context;
        public TicketScheduler(IMongoContext context)
        {
            _context = context;
        }


        public Task Schedule(DateTime executionDate, TicketId ticketId, ScheduleId scheduleId)
        {
            return Schedules.InsertOneAsync(_context.Session, new TicketSchedule(executionDate, ticketId.Value, scheduleId.Value));
        }

        public async Task Prolong(DateTime executionDate, TicketId ticketId, ScheduleId scheduleId)
        {
            await Cancel(scheduleId);
            await Schedule(executionDate, ticketId, scheduleId);
        }

        public Task Cancel(ScheduleId scheduleId)
        {
            return Schedules.DeleteOneAsync(_context.Session,x => x.Id == scheduleId.Value);
        }

        private IMongoCollection<TicketSchedule> Schedules => _context.GetCollection<TicketSchedule>();
    }
}
