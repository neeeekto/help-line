using System;

namespace HelpLine.BuildingBlocks.Application.Queries
{
    public class PageData
    {
        public static PageData Make(PagedQuery query)
        {
            return new PageData(query.Page ?? 1, query.PerPage ?? int.MaxValue);
        }

        public int Page { get; }
        public int PerPage { get; }

        public PageData(int page = 0, int perPage = 0)
        {
            Page = page > 0 ? page : 1;
            PerPage = perPage > 0 ? perPage : int.MaxValue;
        }

        public int Skip => (Page - 1) * PerPage;
    }
}
