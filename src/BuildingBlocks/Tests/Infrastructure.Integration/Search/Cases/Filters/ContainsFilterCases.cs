using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class ContainsFilterForNumberSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.IntArray.Contains(1));

        public override IFilter Filter => new ContainsFilter(
            new[] {nameof(TestModel.IntArray)},
            new[] {new ConstantFilterValue(1)}
        );
    }

    public class ContainsFilterWithDiffValueErrorCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => null;

        public override IFilter Filter => new ContainsFilter(
            new[] {nameof(TestModel.IntArray)},
            new[] {new ConstantFilterValue(1), new ConstantFilterValue("1")}
        );

        public override bool WithError => true;
    }
}
