using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Tests.Infrastructure.SeedWork;
using NetArchTest.Rules;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Infrastructure.Tickets
{
    [TestFixture]
    public class TicketMappingTest : TestBase
    {
        public TicketMappingTest()
        {
            MongoMapAndSerializersRegistry.AddSerializers(new[] {InfrastructureAssembly});
            MongoMapAndSerializersRegistry.AddClassMaps(new[] {InfrastructureAssembly});
        }

        [Test]
        public void All_Events_Have_MongoMap()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(TicketEventBase))
                .GetTypes();


            AssertMongoMappers(types);
        }

        [Test]
        public void All_ViewEvents_Have_MongoMap()
        {
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(TicketEventView))
                .GetTypes();


            AssertMongoMappers(types);
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
        public void All_State_Have_MongoMap()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .ResideInNamespaceStartingWith("HelpLine.Modules.Helpdesk.Domain.Tickets.State")
                .And().DoNotInherit(typeof(TypedNumberIdValueBase))
                .And().DoNotInherit(typeof(TypedStringIdValueBase))
                .And().DoNotInherit(typeof(TypedGuidIdValueBase))
                // TODO: Епала костыльная...
                .And().DoNotHaveName("UserChannels")
                .And().DoNotHaveName("UserMeta")
                .GetTypes()
                .Where(t => !t.IsEnum);

            AssertMongoMappers(types);
        }

        [Test]
        public void All_ViewState_Have_MongoMap()
        {
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .ResideInNamespaceStartingWith("HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels")
                .And().DoNotInherit(typeof(TicketEventView))
                .And().AreNotSealed()
                .GetTypes()
                .Where(t => !t.IsEnum);

            AssertMongoMappers(types);
        }


        [Test]
        public void All_MacrosTriggers_Have_MongoMap()
        {
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(ScenarioTriggerBase<>))
                .GetTypes();

            AssertMongoMappers(types);
        }

        [Test]
        public void All_MacrosModels_Have_MongoMap()
        {
            var types = new[] {typeof(Scenario)};
            AssertMongoMappers(types);
        }
    }
}
