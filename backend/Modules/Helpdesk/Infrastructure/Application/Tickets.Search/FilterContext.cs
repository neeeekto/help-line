using System;
using HelpLine.BuildingBlocks.Application;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search;

public class FilterContext
{
    private readonly IExecutionContextAccessor _contextAccessor;

    public FilterContext(IExecutionContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid CurrentOperator => _contextAccessor.UserId;
}
