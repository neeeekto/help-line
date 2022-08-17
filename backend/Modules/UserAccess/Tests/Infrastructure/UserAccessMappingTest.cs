using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Domain.Users;
using HelpLine.Modules.UserAccess.Tests.Infrastructure.SeedWork;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.Tests.Infrastructure
{
    [TestFixture]
    public class UserAccessMappingTest : TestBase
    {
        public UserAccessMappingTest()
        {
            MongoMapAndSerializersRegistry.AddSerializers(new[] {InfrastructureAssembly});
            MongoMapAndSerializersRegistry.AddClassMaps(new[] {InfrastructureAssembly});
        }

        [Test]
        public void All_StrongTypedId_Have_MongoSerializer()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(TypedGuidIdValueBase))
                .Or().Inherit(typeof(TypedNumberIdValueBase))
                .Or().Inherit(typeof(TypedStringIdValueBase))
                .GetTypes();

            AssertMongoSerializers(types);
        }

        [Test]
        public void All_Entities_Have_MongoMap()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity))
                .And().DoNotInherit(typeof(TypedNumberIdValueBase))
                .And().DoNotInherit(typeof(TypedStringIdValueBase))
                .And().DoNotInherit(typeof(TypedGuidIdValueBase))
                .GetTypes()
                .Where(t => !t.IsEnum);

            AssertMongoMappers(types);
        }
    }
}
