using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class InFilterIntCase : FilterCaseBase
    {
        private static IEnumerable<int> Values = new[] {1, 2};
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => Values.Contains(x.Number));

        public override IFilter Filter =>
            new InFilter(new[] {nameof(TestModel.Number)},
                new[] {new ConstantFilterValue(1), new ConstantFilterValue(2)});
    }
}
