using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Services.Jobs.Models;
using MongoDB.Driver;

namespace HelpLine.Services.Jobs.Infrastructure
{
    internal class JobsMongoContext : MongoContext
    {
        public IMongoCollection<Job> Jobs { get; }

        public JobsMongoContext(string connectionStr, string dbName, ICollectionNameProvider nameProvider) : base(connectionStr, dbName, nameProvider)
        {
            Jobs = GetCollection<Job>();
        }
    }
}
