using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Services
{
    public class TicketIdFactory : ITicketIdFactory
    {
        private readonly IMongoContext _mongoContext;

        public TicketIdFactory(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<TicketId> GetNext(ProjectId projectId)
        {
            var collection = _mongoContext.GetCollection<TicketIdCounter>();
            var counter = await collection.Find(x => x.ProjectId == projectId.Value).FirstOrDefaultAsync();
            if (counter == null) throw new ApplicationException($"Counter for Project '{projectId.Value}' not exist!");
            var next = counter.Next().ToString().PadLeft(7, '0');
            var id = new TicketId($"{counter.Number}-{next}");
            await collection.ReplaceOneAsync(x => x.ProjectId == projectId.Value, counter);
            return id;
        }
    }
}
