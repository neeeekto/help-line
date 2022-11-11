using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.BuildingBlocks.Application.Search.Exception;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;
using HelpLine.BuildingBlocks.Infrastructure.Search;
using MongoDB.Driver;
using NUnit.Framework;
using TestContext = HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork.TestContext;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search
{
    [TestFixture]
    public abstract class FilterTestCaseExecutorBase : SearchTestsBase
    {
        [TestCaseSource(typeof(FilterTestCasesProvider), nameof(FilterTestCasesProvider.Cases))]
        public async Task<bool> CheckCase(FilterCaseBase @case)
        {
            var provider = GetProvider();
            var context = new TestContext();
            await Fill(@case.Data);

            if (@case.WithError)
                Assert.CatchAsync<FilterException>(() => provider.Find(@case.Filter, context));
            else
                await Execute(provider, @case.Filter, context, @case);

            return true;
        }

        private async Task Execute(ISearchProvider<TestModel, TestContext> provider, IFilter filter,
            TestContext context,
            FilterCaseBase @case)
        {
            await Check(await provider.Find(filter, context), @case);
            await Check(await provider.Find(filter, context, new[] {new Sort(true, nameof(TestModel.Id))}), @case);
            await Check(await provider.Find(new PageData(), filter, context, new[]
            {
                new Sort(true,
                    nameof(TestModel.Id))
            }), @case);
        }

        private async Task Check(IEnumerable<TestModel> result, FilterCaseBase @case)
        {
            AssertResult(result, @case.ExpectedResult);
            AssertSorting(result, @case.ExpectedResult);
        }

        private async Task Check(PagedResult<TestModel> result, FilterCaseBase @case)
        {
            AssertResult(result.Result, @case.ExpectedResult);
            AssertSorting(result.Result, @case.ExpectedResult);
            Assert.AreEqual(result.Total, @case.ExpectedResult.Count());
        }

        protected abstract Task Fill(IEnumerable<TestModel> data);
    }
}
