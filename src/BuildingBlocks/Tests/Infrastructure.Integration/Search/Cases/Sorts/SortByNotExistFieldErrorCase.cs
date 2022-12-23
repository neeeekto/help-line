using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Sorts
{
    public class SortByNotExistFieldErrorCase : SortCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => null!;
        public override IEnumerable<Sort> Sorts => new[] {new Sort(true, "unknown")};
        public override bool WithError => true;
    }
}
