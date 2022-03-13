using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public static class MongoMapAndSerializersRegistry
    {
        private static List<Type> _serializers = new List<Type>();
        private static List<Type> _mappers = new List<Type>();
        static MongoMapAndSerializersRegistry()
        {
            /*ConventionRegistry.Register("HelpLine",
                new ConventionPack
                {
                    //new ImmutableTypeClassMapConvention(),
                    new ReadWriteMemberFinderConvention()
                }, _ => true);*/
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            AddSerializers(new[] {typeof(MongoMapAndSerializersRegistry).Assembly});
            AddClassMaps(new[] {typeof(MongoMapAndSerializersRegistry).Assembly});
        }

        public static void AddSerializers(Assembly[] assemblies)
        {
            var serializers = assemblies.SelectMany(x => x.GetTypes())
                .Where(t => t.BaseType != null &&
                            t.BaseType.IsGenericType &&
                            (t.BaseType.GetGenericTypeDefinition() == typeof(SerializerBase<>)
                             || t.BaseType.GetGenericTypeDefinition() == typeof(DictionarySerializerBase<>)
                             || t.BaseType.GetGenericTypeDefinition() == typeof(ReadOnlyDictionaryInterfaceImplementerSerializer<,,>)
                             || t.BaseType.GetGenericTypeDefinition() == typeof(ReadOnlyCollectionSerializer<>)
                            ));

            foreach (var serializerType in serializers)
            {
                if (serializerType.GetCustomAttribute<SpecificMongoSerializer>() != null)
                    continue;
                if(_serializers.Contains(serializerType))
                    continue;
                _serializers.Add(serializerType);
                dynamic serializer = Activator.CreateInstance(serializerType)!;
                BsonSerializer.RegisterSerializer(serializer);
            }
        }

        public static void AddClassMaps(Assembly[] assemblies)
        {
            var mappers = assemblies.SelectMany(x => x.GetTypes())
                .Where(t =>
                    t.BaseType != null &&
                    t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(BsonClassMap<>));

            foreach (var map in mappers)
            {
                if(_mappers.Contains(map))
                    continue;

                _mappers.Add(map);
                dynamic classMap = Activator.CreateInstance(map)!;
                BsonClassMap.RegisterClassMap(classMap);
            }
        }
    }
}
