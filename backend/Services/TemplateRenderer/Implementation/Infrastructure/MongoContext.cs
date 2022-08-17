using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Services.TemplateRenderer.Models;
using MongoDB.Driver;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    internal class MongoContext : BuildingBlocks.Infrastructure.Data.MongoContext
    {
        public IMongoCollection<Component> Components { get; }
        public IMongoCollection<Context> Contexts { get; }
        public IMongoCollection<Template> Templates { get; }

        public MongoContext(string connectionStr, string dbName, ICollectionNameProvider nameProvider) :
            base(
                connectionStr, dbName, nameProvider)
        {
            Components = GetCollection<Component>();
            Contexts = GetCollection<Context>();
            Templates = GetCollection<Template>();
        }
    }
}
