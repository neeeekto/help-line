using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.Mongo;

internal class TicketMongoSearchProvider : ITicketSearchProvider
{
    private readonly MongoFilterBuilder _filterBuilder;
    private readonly MongoSortBuilder _sortBuilder;
    private readonly IMongoContext _mongoContext;


    public TicketMongoSearchProvider(IExecutionContextAccessor accessor, IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
        _filterBuilder = new MongoFilterBuilder(new FilterContext(accessor));
        _sortBuilder = new MongoSortBuilder();
    }

    public async Task<PagedResult<TicketView>> Find(PageData pageData, TicketFilterBase? filter,
        IEnumerable<TicketSortBase> sorts)
    {
        var query = _mongoContext.GetCollection<TicketView>().Find(_filterBuilder.Build(filter));
        var count = await query.CountDocumentsAsync();
        var data = await query
            .Sort(_sortBuilder.Build(sorts))
            .Skip(pageData.Skip).Limit(pageData.PerPage)
            .ToListAsync();
        return new PagedResult<TicketView>()
        {
            Result = data,
            Total = count,
            PageData = pageData
        };
    }

    public async Task<IEnumerable<TicketView>> Find(TicketFilterBase? filter,
        IEnumerable<TicketSortBase> sorts)
    {
        var query = _mongoContext.GetCollection<TicketView>().Find(_filterBuilder.Build(filter));
        return await query
            .Sort(_sortBuilder.Build(sorts))
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> FindIds(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts)
    {
        var query = _mongoContext.GetCollection<TicketView>().Find(_filterBuilder.Build(filter));
        var data = await query
            .Sort(_sortBuilder.Build(sorts))
            .Project(x => x.Id)
            .ToListAsync();
        return data;
    }
}
