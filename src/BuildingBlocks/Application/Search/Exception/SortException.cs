namespace HelpLine.BuildingBlocks.Application.Search.Exception
{
    public class SortException : System.Exception
    {
        public Sort Sort { get; }

        public SortException(Sort sort, System.Exception? innerException = null) : base("SortException", innerException)
        {
            Sort = sort;
        }
    }
}
