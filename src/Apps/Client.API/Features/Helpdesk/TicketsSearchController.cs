using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Utils;
using HelpLine.Apps.Client.API.Features.Helpdesk.Requests;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketFilter;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketFilter;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketFilters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.FindTickets;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features.Helpdesk
{
    [Route("v1/hd/tickets/search")]
    [ApiController]
    [Authorize]
    public class TicketsSearchController : ControllerBase
    {
        private readonly IHelpdeskModule _helpdeskModule;
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public TicketsSearchController(IHelpdeskModule helpdeskModule,
            IExecutionContextAccessor executionContextAccessor)
        {
            _helpdeskModule = helpdeskModule;
            _executionContextAccessor = executionContextAccessor;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<PagedResult<TicketView>>> Search([ProjectParam] string project,
            [FromQuery] int page,
            [FromQuery] int perPage,
            [FromBody] SearchTicketRequest request)
        {
            var pageData = new PageData(page, perPage);
            var filters = new List<IFilter>()
            {
                new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(project),
                    nameof(TicketView.ProjectId))
            };

            if (request.Filter is not null)
            {
                filters.Add(request.Filter);
            }

            var result =
                await _helpdeskModule.ExecuteQueryAsync(new FindTicketsQuery(pageData,
                    new GroupFilter(GroupFilterOperators.And, filters.ToArray()), request.Sorts));
            return Ok(result);
        }

        [HttpGet]
        [Route("filters")]
        public async Task<ActionResult<TicketFilter>> GetFilters([ProjectParam] string project,
            [FromQuery] IEnumerable<TicketFilterFeatures>? features)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetTicketFiltersQuery(project, _executionContextAccessor.UserId, features)));
        }

        [HttpGet]
        [Route("filters/{filterId:guid}")]
        public async Task<ActionResult<TicketFilter>> GetFilter(Guid filterId)
        {
            return Ok(await _helpdeskModule.ExecuteQueryAsync(new GetTicketFilterQuery(filterId)));
        }

        [HttpPost]
        [Route("filters")]
        public async Task<ActionResult<Guid>> AddFilter([ProjectParam] string project,
            [FromBody] TicketFilterData request)
        {
            return Ok(await _helpdeskModule.ExecuteCommandAsync(new SaveTicketFilterCommand(project, request)));
        }

        [HttpPatch]
        [Route("filters/{filterId:guid}")]
        public async Task<ActionResult> UpdateFilter([ProjectParam] string project,
            [FromBody] TicketFilterData request, Guid filterId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new SaveTicketFilterCommand(project, request, filterId));
            return Ok();
        }

        [HttpDelete]
        [Route("filters/{filterId:guid}")]
        public async Task<ActionResult> RemoveFilter(Guid filterId)
        {
            await _helpdeskModule.ExecuteCommandAsync(new RemoveTicketFilterCommand(filterId));
            return Ok();
        }
    }
}
