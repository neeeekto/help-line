using MongoDB.Bson.Serialization;

namespace HelpLine.BuildingBlocks.Infrastructure.InternalCommands
{
    public class InternalCommandTaskMap : BsonClassMap<InternalCommandTaskBase>
    {
        public InternalCommandTaskMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
        }
    }
}
