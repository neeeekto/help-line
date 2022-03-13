using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts
{
    public abstract class SortCaseBase : CaseBase
    {
        public abstract IEnumerable<Sort> Sorts { get; }
        public virtual IFilter? Filter { get; } = null;
        public virtual bool WithError { get; set; }

        protected SortCaseBase(IEnumerable<TestModel> data) : base(data)
        {
        }

        protected SortCaseBase()
        {
        }
    }
}
