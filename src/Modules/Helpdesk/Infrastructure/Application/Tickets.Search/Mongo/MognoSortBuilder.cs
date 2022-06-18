using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.Mongo;

internal class MongoSortBuilder
{
    private readonly SortDefinitionBuilder<TicketView> _sortBuilder = new SortDefinitionBuilder<TicketView>();
    public SortDefinition<TicketView> Build(IEnumerable<TicketSortBase> sorts)
    {
        return _sortBuilder.Combine(sorts.Select(Build));
    }
    
    public SortDefinition<TicketView> Build(TicketSortBase build)
    {

        Expression<Func<TicketView, object>> expr = build switch
        {
            TicketIdSort => x => x.Id,
            TicketCreateDateSort => x => x.CreateDate,
            _ => throw new ArgumentOutOfRangeException(nameof(build), build, null)
        };
        return build.Descending ? _sortBuilder.Descending(expr) : _sortBuilder.Ascending(expr);
    }
}
