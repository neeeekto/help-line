using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Search
{
    public class InFilter : IFilter
    {
        public IEnumerable<string> Path { get; set; }
        public IEnumerable<FilterValue> Values { get; set; }

        public InFilter(IEnumerable<string> path, IEnumerable<FilterValue> values)
        {
            Path = path;
            Values = values;
        }
    }
}
