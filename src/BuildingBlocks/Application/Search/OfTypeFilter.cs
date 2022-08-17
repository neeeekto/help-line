namespace HelpLine.BuildingBlocks.Application.Search
{
    public class OfTypeFilter : IFilter
    {
        public string Type { get; set; }
        public IFilter? Filter { get; set; }

        public OfTypeFilter()
        {
        }

        public OfTypeFilter(string type, IFilter? filter = null)
        {
            Type = type;
            Filter = filter;
        }
    }
}
