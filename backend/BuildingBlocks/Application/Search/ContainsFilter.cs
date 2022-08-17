using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Search
{
    public class ContainsFilter : IFilter
    {
        public IEnumerable<string> Path { get; set; }
        public IEnumerable<FilterValue> Values { get; set; }

        public ContainsFilter(IEnumerable<string> path, IEnumerable<FilterValue> values)
        {
            Path = path;
            Values = values;
        }
    }
}
