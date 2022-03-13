using HelpLine.BuildingBlocks.Application.TypeDescription;

namespace HelpLine.BuildingBlocks.Application.UnitTests.TypeDescription
{
    public class TestClassDescriptionMap : DescriptionClassMap<TestClass>
    {
        public override void Init()
        {
            MapField(x => x.Boolean).SetName("TestBool").SetDescription("test");
            MapField(x => x.Date).SetName("TestDate").SetDescription("test");
            MapField(x => x.Enum).SetName("TestEnum").SetDescription("test");
            MapField(x => x.String).SetName("TestString").SetDescription("test");
            MapField(x => x.StringList).SetName("StringList").SetDescription("test");
            MapField(x => x.EnumList).SetName("EnumList").SetDescription("test");
            MapField(x => x.Deep1.Property);
            MapField(x => x.Deep2);
        }
    }

    public class TestSubClassDescriptionMap : DescriptionClassMap<TestSubClass>
    {
        public override void Init()
        {
            MapField(x => x.List);
            MapField(x => x.Property);
        }
    }

}
