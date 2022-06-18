using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.ElasticSearch;
using HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.Mongo;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search
{
    internal class TicketSearchProvider : ITicketSearchProvider
    {
        private readonly TicketMongoSearchProvider _dbSearch;
        private readonly TicketElasticSearchProvider _elasticSearch;
        private readonly ILogger _logger;

        public TicketSearchProvider(TicketMongoSearchProvider dbSearch, TicketElasticSearchProvider elasticSearch, ILogger logger)
        {
            _dbSearch = dbSearch;
            _elasticSearch = elasticSearch;
            _logger = logger;
        }

        public async Task<PagedResult<TicketView>> Find(PageData pageData, TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts)
        {
            try
            {
                return await _elasticSearch.Find(pageData, filter, sorts);
            }
            catch (Exception e)
            {
                _logger.Warning(e, $"Error using Elastic search");
                return await _dbSearch.Find(pageData, filter, sorts);
            }
        }

        public async Task<IEnumerable<TicketView>> Find(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts)
        {
            try
            {
                return await _elasticSearch.Find(filter, sorts);
            }
            catch (Exception e)
            {
                _logger.Warning(e, $"Error using Elastic search");
                return await _dbSearch.Find(filter, sorts);
            }
        }

        public async Task<IEnumerable<string>> FindIds(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts)
        {
            try
            {
                return await _elasticSearch.FindIds(filter, sorts);
            }
            catch (Exception e)
            {
                _logger.Warning(e, $"Error using Elastic search");
                return await _dbSearch.FindIds(filter, sorts);
            }
        }
    }
}
