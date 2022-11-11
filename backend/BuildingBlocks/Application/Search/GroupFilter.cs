using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Search
{
    public class GroupFilter : IFilter
    {
        public GroupFilterOperators Operation { get; set; }
        public IEnumerable<IFilter> Filters { get; set; }

        public GroupFilter()
        {
        }

        public GroupFilter(GroupFilterOperators operation, params IFilter[] filters)
        {
            Operation = operation;
            Filters = filters;
        }
    }
}
