using HelpLine.BuildingBlocks.Application.TypeDescription;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Application.UnitTests.TypeDescription
{
    [TestFixture]
    public class TypeDescriptionTests
    {
        [Test]
        public void Debug_Test()
        {
            var desc = new Description(new DescriptionClassMap[]
            {
                new TestClassDescriptionMap(),
                new TestSubClassDescriptionMap(),
            }, typeof(TestClass));
        }
    }
}
