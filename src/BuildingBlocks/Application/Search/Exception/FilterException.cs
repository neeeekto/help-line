namespace HelpLine.BuildingBlocks.Application.Search.Exception
{
    public class FilterException : System.Exception
    {
        public IFilter? Filter { get; }

        public FilterException(IFilter? filter, System.Exception? innerException = null) : base("", innerException)
        {
            Filter = filter;
        }
    }
}
