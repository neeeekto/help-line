using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases
{
    public abstract class CaseBase
    {
        public abstract IEnumerable<TestModel> ExpectedResult { get; }
        public readonly IEnumerable<TestModel> Data;
        public virtual string? Descritpion { get; set; }

        protected CaseBase(IEnumerable<TestModel> data)
        {
            Data = data;
        }

        protected CaseBase()
        {
            Data = TestModelFactory.Make(10);
        }
    }
}
