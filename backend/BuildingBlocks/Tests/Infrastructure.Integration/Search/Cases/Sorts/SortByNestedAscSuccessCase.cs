using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts
{
    public class SortByNestedAscSuccessCase : SortCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.OrderBy(x => x.Item.DateTime);

        public override IEnumerable<Sort> Sorts => new[]
            {new Sort(true, nameof(TestModel.Item), nameof(TestModel.Item.DateTime))};
    }
}
