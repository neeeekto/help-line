using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.Cases.Filters
{
    public class ElementMathByValueFilterCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.Items.Any(x => x.Number == 2));

        public override IFilter Filter => new ElementMathFilter(
            new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(1), nameof(TestModelItem.Number)),
            nameof(TestModel.Items)
        );
    }


    public class ElementMathByTypeFilterCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.Items.OfType<TestModelItemChild1>().Any());

        public override IFilter Filter => new ElementMathFilter(
            new OfTypeFilter(nameof(TestModelItemChild1)),
            nameof(TestModel.Items)
        );
    }

    public class ElementMathByDictionaryCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.Dictionary.ContainsKey(TestModelFactory.GetString(1)));

        public override IFilter Filter => new ElementMathFilter(
            new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(TestModelFactory.GetString(1)), "Key"),
            nameof(TestModel.Dictionary)
        );
    }

    public class ElementMathByTypeAndValueFilterCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.Items.OfType<TestModelItemChild1>().Any(x => x.Number == 1));

        public override IFilter Filter => new ElementMathFilter(
            new OfTypeFilter(nameof(TestModelItemChild1),
                new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(1),
                    nameof(TestModelItemChild1.Number))
            ),
            nameof(TestModel.Items)
        );
    }

    public class ElementMathByOrTypeAndValueFilterCase : FilterCaseBase
    {
        public override IEnumerable<TestModel> ExpectedResult =>
            Data.Where(x => x.Items.OfType<TestModelItemChild1>().Any(x => x.Number == 1)
                            || x.Items.OfType<TestModelItemChild2>()
                                .Any(x => x.Desc == TestModelFactory.GetString(1, 1))
            );

        public override IFilter Filter => new ElementMathFilter(
            new GroupFilter(GroupFilterOperators.Or,
                new OfTypeFilter(nameof(TestModelItemChild1),
                    new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(1),
                        nameof(TestModelItemChild1.Number)
                    )
                ),
                new OfTypeFilter(nameof(TestModelItemChild2),
                    new ValueFilter(FieldFilterOperators.Equal,
                        new ConstantFilterValue(TestModelFactory.GetString(1, 1)),
                        nameof(TestModelItemChild2.Desc)
                    )
                )
            ),
            nameof(TestModel.Items)
        );
    }
}
