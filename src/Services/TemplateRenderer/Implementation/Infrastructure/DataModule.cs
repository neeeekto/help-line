using Autofac;
using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    internal class DataModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;
        private readonly string _databaseName;

        public DataModule(string databaseConnectionString, string databaseName)
        {
            _databaseConnectionString = databaseConnectionString;
            _databaseName = databaseName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            MongoMapAndSerializersRegistry.AddSerializers(new[] {ServiceInfo.Assembly});
            MongoMapAndSerializersRegistry.AddClassMaps(new[] {ServiceInfo.Assembly});


            builder.RegisterType<MongoContext>()
                .As<IMongoContext>()
                .AsSelf()
                .WithParameter("connectionStr", _databaseConnectionString)
                .WithParameter("dbName", _databaseName)
                .SingleInstance();

            builder.RegisterType<CollectionNameProvider>()
                .As<ICollectionNameProvider>()
                .SingleInstance();
        }
    }
}
