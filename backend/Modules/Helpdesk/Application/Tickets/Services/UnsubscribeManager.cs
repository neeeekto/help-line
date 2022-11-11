using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Services
{
    public class UnsubscribeManager : IUnsubscribeManager
    {
        private readonly IMongoContext _context;
        private readonly IRepository<Unsubscribe> _repository;

        public UnsubscribeManager(IMongoContext context, IRepository<Unsubscribe> repository)
        {
            _context = context;
            _repository = repository;
        }


        public Task TryRemove(UserId userId, ProjectId projectId)
        {
            return _repository.Remove(x => x.UserId == userId.Value && x.ProjectId == projectId.Value);
        }

        public Task Add(UserId userId, ProjectId projectId, string message)
        {
            return _repository.Add(new Unsubscribe
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Message = message,
                ProjectId = projectId.Value,
                UserId = userId.Value
            });
        }

        public Task<bool> CheckExist(UserId userId, ProjectId projectId)
        {
            return _context.GetCollection<Unsubscribe>()
                .Find(x => x.ProjectId == projectId.Value && x.UserId == projectId.Value).AnyAsync();
        }
    }
}
