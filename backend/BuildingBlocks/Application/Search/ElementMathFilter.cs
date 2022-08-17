using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Search
{
    public class ElementMathFilter : IFilter
    {
        public IEnumerable<string> Path { get; set; }
        public IFilter ItemFilter { get; set; }

        public ElementMathFilter()
        {
        }

        public ElementMathFilter(IFilter itemFilter, params string[] path)
        {
            Path = path;
            ItemFilter = itemFilter;
        }
    }
}
