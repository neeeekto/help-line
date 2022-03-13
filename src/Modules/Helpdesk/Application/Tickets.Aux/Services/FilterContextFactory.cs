using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Services
{
    public class FilterContextFactory : IFilterContextFactory
    {
        private readonly IExecutionContextAccessor _contextAccessor;

        public FilterContextFactory(IExecutionContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Task<TicketFilterCtx> Make()
        {
            return Task.FromResult(new TicketFilterCtx
            {
                CurrentUser = _contextAccessor.UserId,
                Now = DateTime.Now
            });
        }
    }
}
