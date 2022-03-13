using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;
using HelpLine.BuildingBlocks.Infrastructure.Search;
using NUnit.Framework;
using TestContext = HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork.TestContext;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search
{
    [TestFixture]
    public abstract class SearchTestsBase
    {

        protected void AssertResult(IEnumerable<TestModel> expected, IEnumerable<TestModel> actual)
        {
            Assert.AreEqual(expected.Count(), actual.Count());
            Assert.IsTrue(expected.All(x => actual.Any(z => z.Id == x.Id)));
        }

        protected void AssertSorting(IEnumerable<TestModel> expected, IEnumerable<TestModel> actual)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();
            for (int i = 0; i < expectedList.Count(); i++)
            {
                var target = expectedList[i];
                var act = actualList[i];
                Assert.AreEqual(target.Id, act.Id);
            }
        }

        protected abstract ISearchProvider<TestModel, TestContext> GetProvider();
    }
}
