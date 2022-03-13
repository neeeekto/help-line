using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Services
{
    public class TicketConfigurations : ITicketConfigurations
    {
        private readonly IMongoContext _context;

        public TicketConfigurations(IMongoContext context)
        {
            _context = context;
        }

        public async Task<TimeSpan> GetLifeCycleDelay(ProjectId projectId, TicketLifeCycleType type)
        {
            var conf = await Get(projectId);
            return conf.LifeCycleDelay[type];
        }

        public async Task<TimeSpan> GetInactivityDelay(ProjectId projectId)
        {
            var conf = await Get(projectId);
            return conf.InactivityDelay;
        }

        public async Task<TimeSpan> GetFeedbackCompleteDelay(ProjectId scope)
        {
            var conf = await Get(scope);
            return conf.FeedbackCompleteDelay;
        }

        private IMongoCollection<TicketsDelayConfiguration> Collection => _context.GetCollection<TicketsDelayConfiguration>();

        private async Task<TicketsDelayConfiguration> Get(ProjectId projectId)
        {
            return await Collection.Find(x => x.ProjectId == projectId.Value).FirstAsync();
        }
    }
}
