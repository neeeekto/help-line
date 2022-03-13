using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class OrFilterWithOneFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Bool);

        public override IFilter Filter => new GroupFilter(
            GroupFilterOperators.Or,
            new ValueFilter(FieldFilterOperators.Equal,
                new ConstantFilterValue(true), nameof(TestModel.Bool))
        );
    }

    public class OrFilterWithTwoBoolFiltersSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Bool || !x.Bool);

        public override IFilter Filter => new GroupFilter(
            GroupFilterOperators.Or,
            new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(true), nameof(TestModel.Bool)),
            new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(false), nameof(TestModel.Bool))
        );
    }
}
