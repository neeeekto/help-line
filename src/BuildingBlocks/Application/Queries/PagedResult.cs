using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.Queries
{
    public class PagedResult<T>
    {
        public PageData PageData { get; set; }
        public long Total { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
