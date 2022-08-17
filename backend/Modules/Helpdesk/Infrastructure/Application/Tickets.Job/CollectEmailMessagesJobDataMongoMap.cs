using HelpLine.Modules.Helpdesk.Jobs;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Job
{
    internal class CollectEmailMessagesJobDataMongoMap : BsonClassMap<CollectEmailMessagesJobData>
    {
        public CollectEmailMessagesJobDataMongoMap()
        {
            SetDiscriminator(nameof(CollectEmailMessagesJobData));
            AutoMap();
        }
    }
}
