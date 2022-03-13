using HelpLine.Modules.Helpdesk.ArchTests.SeedWork;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.ArchTests
{
    [TestFixture]
    public class InfrastructureTests  : TestBase
    {
        [Test]
        public void AllMapper_Should_Have_Name_EndingWith_Map()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .Inherit(typeof(BsonClassMap<>))
                .Should()
                .HaveNameEndingWith("Map")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void AllSerializer_Should_Have_Name_EndingWith_Serializer()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .Inherit(typeof(SerializerBase<>))
                .Should()
                .HaveNameEndingWith("Serializer")
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void AllMapper_Should_Not_Be_Public()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .Inherit(typeof(BsonClassMap<>))
                .Should()
                .NotBePublic()
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void AllSerializer_Should_Not_Be_Public()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .Inherit(typeof(SerializerBase<>))
                .Should()
                .NotBePublic()
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}
