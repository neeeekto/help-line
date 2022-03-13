using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Search.Exception;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;
using NUnit.Framework;
using TestContext = HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork.TestContext;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search
{
    public abstract class SortTestCaseExecutorBase : SearchTestsBase
    {
        [TestCaseSource(typeof(SortTestCasesProvider), nameof(SortTestCasesProvider.Cases))]
        public async Task<bool> CheckCase(SortCaseBase @case)
        {
            var provider = GetProvider();
            var context = new TestContext();
            await Fill(@case.Data);
            if (@case.WithError)
                Assert.CatchAsync<SortException>(() => provider.Find(@case.Filter, context, @case.Sorts));
            else
                await Check(await provider.Find(@case.Filter, context, @case.Sorts), @case);

            return true;
        }

        private async Task Check(IEnumerable<TestModel> result, SortCaseBase @case)
        {
            AssertResult(result, @case.ExpectedResult);
            AssertSorting(result, @case.ExpectedResult);
        }

        protected abstract Task Fill(IEnumerable<TestModel> data);
    }
}
