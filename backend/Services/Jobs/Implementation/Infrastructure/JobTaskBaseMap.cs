using HelpLine.Services.Jobs.Contracts;
using MongoDB.Bson.Serialization;

namespace HelpLine.Services.Jobs.Infrastructure
{
    public class JobTaskBaseMap : BsonClassMap<JobTask>
    {
        public JobTaskBaseMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
        }
    }
}
