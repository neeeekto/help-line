using HelpLine.BuildingBlocks.Application.Queries;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Tests.Application.Unit.Queries
{
    [TestFixture]
    public class PagedQueryHelperTests
    {
        [TestCase(1, 5, 0)]
        [TestCase(0, 5, 0)]
        [TestCase(2, 5, 5)]
        [TestCase(null, 5, 0)]
        [TestCase(null, null, 0)]
        public void PagedQueryHelper_GetPageData_Test(int? page, int? perPage, int skip)
        {
            var data = PageData.Make(new PagedQuery {Page = page, PerPage = perPage});
            Assert.AreEqual(skip, data.Skip);
        }
    }
}
