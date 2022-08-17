using HelpLine.BuildingBlocks.Application.TypeDescription;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Tests.Application.Unit.TypeDescription
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
