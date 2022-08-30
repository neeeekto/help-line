using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Infrastructure.UnitTests.Search
{
    public class NestedContext
    {
        public static T Fill<T>(T ctx) where T : NestedContext
        {
            ctx.Bool = true;
            ctx.Number = 1;
            ctx.String = "string";
            ctx.StringArray = new[] {"test"};
            ctx.StringEnumerable = new List<string>() {"test"};

            return ctx;
        }

        public string String { get; set; }
        public int Number { get; set; }
        public bool Bool { get; set; }
        public string[] StringArray { get; set; }
        public IEnumerable<string> StringEnumerable { get; set; }
    }

    public class TestContext : NestedContext
    {
        public static TestContext Make()
        {
            var nested = Fill(new NestedContext());
            var ctx = Fill(new TestContext());
            ctx.Nested = nested;
            ctx.Id = "id";
            ctx.NestedArrays = new[] {Fill(new NestedContext())};
            return ctx;
        }

        public string Id { get; set; }
        public NestedContext Nested { get; set; }
        public NestedContext[] NestedArrays { get; set; }
    }
}
