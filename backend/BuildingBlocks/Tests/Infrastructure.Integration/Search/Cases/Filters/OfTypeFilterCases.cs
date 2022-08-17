using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class OfTypeWithValueFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.OfType<TestModelExtend>().Where(x => x.Long == 2);

        public override IFilter Filter => new OfTypeFilter(nameof(TestModelExtend),
            new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(2), nameof(TestModelExtend.Long))
        );

        public OfTypeWithValueFilterSuccessCase() : base(new[]
        {
            new TestModel() {Id = "1"},
            new TestModelExtend() {Id = "2", Long = 2},
            new TestModelExtend() {Id = "3", Long = 1},
            new TestModelExtend() {Id = "4", Long = 3},
        })
        {
        }
    }

    public class OfTypeFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.OfType<TestModelExtend>();
        public override IFilter Filter => new OfTypeFilter(nameof(TestModelExtend));

        public OfTypeFilterSuccessCase() : base(new TestModel[]
        {
            new TestModel() {Id = "1"},
            new TestModelExtend() {Id = "2"},
        })
        {
        }
    }
}
