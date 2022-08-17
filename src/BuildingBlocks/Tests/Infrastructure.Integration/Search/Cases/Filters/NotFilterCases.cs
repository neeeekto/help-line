using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class NotFilterWithBoolFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => !x.Bool);

        public override IFilter Filter => new NotFilter(new ValueFilter(
            FieldFilterOperators.Equal,
            new ConstantFilterValue(true),
            nameof(TestModel.Bool)
        ));
    }

    public class NotFilterWithNumberFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number != 1);

        public override IFilter Filter => new NotFilter(
            new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(1), nameof(TestModel.Number)
            ));
    }

    public class NotFilterWithStringFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.String != TestModelFactory.GetString(1));

        public override IFilter Filter => new NotFilter(
            new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(TestModelFactory.GetString(1)),
                nameof(TestModel.String)
            ));
    }

    public class NotFilterWithAndFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x =>
            !(x.Number == 1 && x.String == TestModelFactory.GetString(1)));

        public override IFilter Filter => new NotFilter(
            new GroupFilter(
                GroupFilterOperators.And,
                new[]
                {
                    new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(1), nameof(TestModel.Number)),
                    new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(TestModelFactory.GetString(1)),
                        nameof(TestModel.String)),
                }
            )
        );
    }

    public class NotFilterWithOrFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x =>
            !(x.Number == 1 || x.Number == 2));

        public override IFilter Filter => new NotFilter(
            new GroupFilter(
                GroupFilterOperators.Or,
                new[]
                {
                    new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(1), nameof(TestModel.Number)),
                    new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(2), nameof(TestModel.Number)),
                }
            )
        );
    }
}
