using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Infrastructure;
using MongoDB.Bson.Serialization;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Infrastructure.SeedWork
{
    public abstract class TestBase
    {
        protected static Assembly ApplicationAssembly = typeof(CommandBase).Assembly;
        protected static Assembly DomainAssembly = typeof(Ticket).Assembly;
        protected static Assembly InfrastructureAssembly = typeof(HelpdeskModule).Assembly;

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            Assert.That(types, Is.Null.Or.Empty);
        }

        protected static void AssertMongoMappers(IEnumerable<Type> types)
        {
            var mappers = BsonClassMap.GetRegisteredClassMaps().ToList();
            var failingTypes = new List<Type>();

            foreach (var type in types)
            {
                if (mappers.All(x => x.ClassType != type))
                    failingTypes.Add(type);
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertMongoSerializers(IEnumerable<Type> types)
        {
            var failingTypes = new List<Type>();

            foreach (var type in types)
            {
                var serializer = BsonSerializer.SerializerRegistry.GetSerializer(type);

                // Да, мы чекаем именно так, потому что если я не регаю свой он будет от драйвера
                if (serializer.GetType().Assembly != InfrastructureAssembly)
                    failingTypes.Add(type);
            }

            AssertFailingTypes(failingTypes);
        }
    }
}
