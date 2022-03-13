using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Search
{
    public class ValueFilter : IFilter
    {
        public IEnumerable<string> Path { get; set; }
        public FieldFilterOperators Operator { get; set; }
        public FilterValue Value { get; set; }

        public ValueFilter()
        {
        }

        public ValueFilter(FieldFilterOperators @operator, FilterValue value, params string[] path)
        {
            Path = path;
            Operator = @operator;
            Value = value;
        }
    }
}
