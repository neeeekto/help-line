namespace HelpLine.BuildingBlocks.Application.Queries
{
    public class PagedQuery
    {
        /// <summary>
        /// Page number. If null then default is 1.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Records number per page (page size).
        /// </summary>
        public int? PerPage { get; set; }
    }
}
