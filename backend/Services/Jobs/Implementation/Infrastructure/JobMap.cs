using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Jobs.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Services.Jobs.Infrastructure
{
    internal class JobMap : BsonClassMap<Job>
    {
        public JobMap()
        {
            MapIdMember(x => x.Id);
            MapIdMember(x => x.Data);
            AutoMap();
        }
    }

    internal class JobDataBaseMap : BsonClassMap<JobDataBase>
    {
        public JobDataBaseMap()
        {
            AutoMap();
            SetIsRootClass(true);
        }
    }
}
