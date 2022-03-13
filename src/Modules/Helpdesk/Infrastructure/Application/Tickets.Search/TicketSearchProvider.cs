using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.Search.Mongo;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search
{
    internal class TicketSearchProvider : MongoSearchProvider<TicketView, TicketFilterCtx>
    {
        public TicketSearchProvider(IFilterValueGetter filterValueGetter, IMongoContext ctx,
            IAdditionalTypeProvider typeProvider, IValueMapper valueMapper) : base(filterValueGetter,
            ctx.GetCollection<TicketView>(), typeProvider, valueMapper)
        {
        }
    }
}
