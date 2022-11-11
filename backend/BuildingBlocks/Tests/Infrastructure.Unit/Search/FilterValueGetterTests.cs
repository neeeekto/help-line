using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Expressions;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Infrastructure.Search;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Infrastructure.UnitTests.Search
{
    [TestFixture]
    public class FilterValueGetterTests
    {
        [TestCase("string")]
        [TestCase(1)]
        [TestCase(1.1)]
        [TestCase(true)]
        public async Task GetConstantValue_Success(object value)
        {
            var ctx = new TestContext();
            var getter = new FilterValueGetter<TestContext>();
            var filterValue = new ConstantFilterValue {Value = value};

            var result = getter.Get(filterValue, ctx);

            Assert.AreEqual(value, result);
        }

        [Test]
        public async Task GetConstantValue_ForArray_Success()
        {
            var ctx = new TestContext();
            var getter = new FilterValueGetter<TestContext>();
            var filterValue = new ConstantFilterValue {Value = new[] {"test"}};

            var result = getter.Get(filterValue, ctx);

            Assert.AreEqual(filterValue.Value, result);
        }

        [TestCase(new object[] {nameof(TestContext.Id)})]
        [TestCase(new object[] {nameof(TestContext.Bool)})]
        [TestCase(new object[] {nameof(TestContext.Number)})]
        [TestCase(new object[] {nameof(TestContext.String)})]
        [TestCase(new object[] {nameof(TestContext.Nested), nameof(NestedContext.String)})]
        [TestCase(new object[] {nameof(TestContext.Nested), nameof(NestedContext.Bool)})]
        [TestCase(new object[] {nameof(TestContext.Nested), nameof(NestedContext.Number)})]
        [TestCase(new object[] {nameof(TestContext.StringArray), "0"})]
        [TestCase(new object[] {nameof(TestContext.Nested), nameof(TestContext.StringArray), "0"})]
        [TestCase(new object[] {nameof(TestContext.NestedArrays), "0", nameof(NestedContext.Number)})]
        [TestCase(new object[] {nameof(TestContext.NestedArrays), "0", nameof(NestedContext.Bool)})]
        [TestCase(new object[] {nameof(TestContext.NestedArrays), "0", nameof(NestedContext.String)})]
        public async Task GetContextValue_Success(object[] path)
        {
            var pathToField = path.Select(x => (string) x).ToList();
            var ctx = TestContext.Make();
            var getter = new FilterValueGetter<TestContext>();
            var filterValue = new ContextFilterValue() {Path = pathToField};

            var fieldGetterExpression = FieldExpressionBuilder.MakeGetter<TestContext>(pathToField, out var parameter);
            var fieldGetter = LamdaBuilder.MakeFuncExpression(typeof(TestContext), fieldGetterExpression.Type,
                fieldGetterExpression, parameter);
            var result = getter.Get(filterValue, ctx);

            var aaa = fieldGetter.Compile().DynamicInvoke(ctx);
            Assert.AreEqual(aaa, result);
        }

        [Test]
        public async Task GetContextValue_ForEnumerable_Error()
        {
            var ctx = TestContext.Make();
            var getter = new FilterValueGetter<TestContext>();
            var filterValue = new ContextFilterValue {Path = new[] {nameof(ctx.StringEnumerable), "0"}};

            Assert.Catch(() => getter.Get(filterValue, ctx));
        }

        [Test]
        public async Task GetContextValue_NotExist_Error()
        {
            var ctx = TestContext.Make();
            var getter = new FilterValueGetter<TestContext>();
            var filterValue = new ContextFilterValue {Path = new[] {"unknown"}};

            Assert.Catch(() => getter.Get(filterValue, ctx));
        }
    }
}
