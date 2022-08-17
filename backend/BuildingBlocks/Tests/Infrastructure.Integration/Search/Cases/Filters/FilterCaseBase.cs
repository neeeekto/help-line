using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public abstract class FilterCaseBase : CaseBase
    {
        public abstract IFilter Filter { get; }
        public virtual bool WithError { get; }

        protected FilterCaseBase(IEnumerable<TestModel> data) : base(data)
        {
        }

        protected FilterCaseBase()
        {
        }
    }
}
