using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts
{
    public class SortByDateDescSuccessCase : SortCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.OrderByDescending(x => x.DateTime);
        public override IEnumerable<Sort> Sorts => new []{new Sort(false, nameof(TestModel.DateTime))};
    }
}
