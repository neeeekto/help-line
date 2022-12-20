using System;
using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class AndFilterWithTwoDiffValueSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.Bool && x.Id == TestModelFactory.GetId(2));

        public override IFilter Filter => new GroupFilter(
            GroupFilterOperators.And,
            new ValueFilter(FieldFilterOperators.Equal,
                new ConstantFilterValue(true), nameof(TestModel.Bool)),
            new ValueFilter(FieldFilterOperators.Equal,
                new ConstantFilterValue(TestModelFactory.GetId(2)), nameof(TestModel.Id))
        );
    }

    public class AndFilterWithOneFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Bool);

        public override IFilter Filter => new GroupFilter(
            GroupFilterOperators.And,
            new ValueFilter(FieldFilterOperators.Equal,
                new ConstantFilterValue(true), nameof(TestModel.Bool))
        );
    }

    public class AndFilterBetweenNumberSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number > 1 && x.Number < 10);

        public override IFilter Filter => new GroupFilter(GroupFilterOperators.And,
            new ValueFilter(FieldFilterOperators.Great, new ConstantFilterValue(1), nameof(TestModel.Number)),
            new ValueFilter(FieldFilterOperators.Less, new ConstantFilterValue(10), nameof(TestModel.Number))
        );
    }

    public class AndFilterBetweenDateSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x =>
            x.DateTime > DateTime.UtcNow.AddHours(-10)
            && x.DateTime < DateTime.UtcNow.AddHours(-5));

        public override IFilter Filter => new GroupFilter(GroupFilterOperators.And,
            new ValueFilter(FieldFilterOperators.Great, new ConstantFilterValue(DateTime.UtcNow.AddHours(-10)),
                nameof(TestModel.DateTime)),
            new ValueFilter(FieldFilterOperators.Less, new ConstantFilterValue(DateTime.UtcNow.AddHours(-5)),
                nameof(TestModel.DateTime))
        );
    }

    public class AndFilterBetweenStringErrorCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => null;

        public override IFilter Filter => new GroupFilter(GroupFilterOperators.And,
            new ValueFilter(FieldFilterOperators.Great, new ConstantFilterValue("1"),
                nameof(TestModel.String)),
            new ValueFilter(FieldFilterOperators.Less, new ConstantFilterValue("2"),
                nameof(TestModel.String))
        );

        public override bool WithError => true;
    }
}
