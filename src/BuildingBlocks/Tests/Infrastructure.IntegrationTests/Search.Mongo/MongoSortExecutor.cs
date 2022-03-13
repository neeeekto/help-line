using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Mongo.SeedWork;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;
using HelpLine.BuildingBlocks.Infrastructure.Search;
using HelpLine.BuildingBlocks.Infrastructure.Search.Mongo;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NUnit.Framework;
using TestContext = HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork.TestContext;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Mongo
{
    [TestFixture]
    public class MongoSortExecutor : SortTestCaseExecutorBase
    {
        protected IMongoContext Ctx;
        protected readonly MongoSearchProvider<TestModel, TestContext> Provider;

        public MongoSortExecutor()
        {
            var nameProvider = new TestNameProvider(nameof(MongoSortExecutor));
            Ctx = new TestMongoContext("mongodb://127.0.0.1", "helpline-tests_building-blocks_search", nameProvider);
            Provider = new MongoSearchProvider<TestModel, TestContext>(
                new FilterValueGetter<TestSearchCtx>(),
                Ctx.GetCollection<TestModel>(),
                new AdditionalTypeProvider(),
                new NoopValueMapper());
        }

        protected async Task Clear()
        {
            await Ctx.GetCollection<TestModel>().DeleteManyAsync(x => true);
        }

        protected override async Task Fill(IEnumerable<TestModel> data)
        {
            await Clear();
            await Ctx.GetCollection<TestModel>().InsertManyAsync(data);
        }

        protected override ISearchProvider<TestModel, TestContext> GetProvider()
        {
            return Provider;
        }
    }
}
