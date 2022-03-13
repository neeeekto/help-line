namespace HelpLine.BuildingBlocks.Application.Search
{
    public class NotFilter : IFilter
    {
        public IFilter Filter { get; set; }

        public NotFilter()
        {
        }

        public NotFilter(IFilter filter)
        {
            Filter = filter;
        }
    }
}
