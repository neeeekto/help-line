using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Search
{
    public abstract class FilterValue
    {
    }

    public class ConstantFilterValue : FilterValue
    {
        public object Value { get; set; }

        public ConstantFilterValue()
        {
        }

        public ConstantFilterValue(object value)
        {
            Value = value;
        }
    }

    public class ContextFilterValue : FilterValue
    {
        public IEnumerable<string> Path { get; set; }

        public ContextFilterValue()
        {
        }

        public ContextFilterValue(params string[] path)
        {
            Path = path;
        }
    }
}
