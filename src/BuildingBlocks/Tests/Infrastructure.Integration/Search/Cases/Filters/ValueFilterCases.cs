using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class NumberValueEqFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number == 1);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(1),
            nameof(TestModel.Number));
    }

    public class NumberValueLessFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number < 5);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Less, new ConstantFilterValue(5),
            nameof(TestModel.Number));
    }

    public class NumberValueLessOrEqualFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number <= 5);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.LessOrEqual, new ConstantFilterValue(5),
            nameof(TestModel.Number));
    }

    public class NumberValueGreatFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number > 5);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Great, new ConstantFilterValue(5),
            nameof(TestModel.Number));
    }

    public class NumberValueGreatOrEqualFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number >= 5);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.GreatOrEqual, new ConstantFilterValue(5),
            nameof(TestModel.Number));
    }

    public class NumberValueNeFilteSuccessrCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Number != 5);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.NotEqual, new ConstantFilterValue(5),
            nameof(TestModel.Number));
    }

    public class NumberFilterWithStringValueErrorCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => null;

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.NotEqual, new ConstantFilterValue("5"),
            nameof(TestModel.Number));

        public override bool WithError => true;
    }

    public class StringValueEqFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.String == TestModelFactory.GetString(1));

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Equal,
            new ConstantFilterValue(TestModelFactory.GetString(1)), nameof(TestModel.String));
    }

    public class StringValueNeFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.String != TestModelFactory.GetString(1));

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.NotEqual,
            new ConstantFilterValue(TestModelFactory.GetString(1)), nameof(TestModel.String));
    }

    public class StringValueWithIntValueFilterErrorCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => null;

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Equal,
            new ConstantFilterValue(1), nameof(TestModel.String));

        public override bool WithError => true;
    }

    public class BoolValueEqFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Bool);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(true),
            nameof(TestModel.Bool));
    }

    public class BoolValueNeFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Bool != true);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.NotEqual, new ConstantFilterValue(true),
            nameof(TestModel.Bool));
    }

    public class DateValueEqFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.DateTime == Data.First().DateTime);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Equal,
            new ConstantFilterValue(Data.First().DateTime), nameof(TestModel.DateTime));
    }

    public class DateValueNeFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.DateTime != Data.First().DateTime);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.NotEqual,
            new ConstantFilterValue(Data.First().DateTime), nameof(TestModel.DateTime));
    }

    public class DateValueLessFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.DateTime < Data.Skip(2).First().DateTime);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Less,
            new ConstantFilterValue(Data.Skip(2).First().DateTime), nameof(TestModel.DateTime));
    }

    public class DateValueLessOrEqualFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.DateTime <= Data.Skip(2).First().DateTime);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.LessOrEqual,
            new ConstantFilterValue(Data.Skip(2).First().DateTime), nameof(TestModel.DateTime));
    }

    public class DateValueGreatFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.DateTime > Data.Skip(2).First().DateTime);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Great,
            new ConstantFilterValue(Data.Skip(2).First().DateTime), nameof(TestModel.DateTime));
    }

    public class DateValueGreatOrEqualFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.DateTime >= Data.Skip(2).First().DateTime);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.GreatOrEqual,
            new ConstantFilterValue(Data.Skip(2).First().DateTime), nameof(TestModel.DateTime));
    }

    public class EnumValueEqFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Enum == TestModelEnum.Enum1);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.Equal,
            new ConstantFilterValue(0), nameof(TestModel.Enum));
    }

    public class EnumValueNeFilterSuccessCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult => Data.Where(x => x.Enum != TestModelEnum.Enum1);

        public override IFilter Filter => new ValueFilter(FieldFilterOperators.NotEqual,
            new ConstantFilterValue(0), nameof(TestModel.Enum));
    }


}
