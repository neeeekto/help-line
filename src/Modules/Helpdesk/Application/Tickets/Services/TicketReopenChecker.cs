using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Services
{
    public class TicketReopenChecker : ITicketReopenChecker
    {
        private readonly IMongoContext _context;

        public TicketReopenChecker(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckBy(TicketFeedback feedback, ProjectId projectId)
        {
            var conditions = await _context.GetCollection<TicketReopenCondition>()
                .Find(x => x.Enabled && x.ProjectId == projectId.Value).ToListAsync();
            var condition = conditions.OrderBy(x => x.Weight).FirstOrDefault();
            if (condition == null)
                return false;

            return CheckByCondition(condition, feedback);
        }

        private bool CheckByCondition(TicketReopenCondition condition, TicketFeedback feedback)
        {
            var result = false;
            if (condition.MustSolved)
                result = feedback.Solved == true;

            if (!result)
                result = feedback.Score > condition.MinimalScore;

            return result;
        }
    }
}
