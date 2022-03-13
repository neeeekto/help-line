using HelpLine.Modules.Helpdesk.Domain.TemporaryProblems;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.TemporaryProblems
{
    internal class TemporaryProblemMap : BsonClassMap<TemporaryProblem>
    {
        public TemporaryProblemMap()
        {
            MapIdField(x => x.Id);
            AutoMap();
            MapField("_statusesDate").SetElementName("StatusesDate");
            MapField("_updates").SetElementName("Updates");
            MapField("_subscribers").SetElementName("Subscribers");
        }
    }

    internal class TemporaryProblemInfoMap : BsonClassMap<TemporaryProblemInfo>
    {
        public TemporaryProblemInfoMap()
        {
            AutoMap();
            MapCreator(x => new TemporaryProblemInfo(x.Title, x.Description, x.InitStatus));
        }
    }

    internal class TemporaryProblemMetaMap : BsonClassMap<TemporaryProblemMeta>
    {
        public TemporaryProblemMetaMap()
        {
            AutoMap();
            MapCreator(x => new TemporaryProblemMeta(x.Name, x.Languages, x.Platforms));
        }
    }

    internal class TemporaryProblemUpdateMap : BsonClassMap<TemporaryProblemUpdate>
    {
        public TemporaryProblemUpdateMap()
        {
            AutoMap();
        }
    }

    internal class TemporaryProblemUpdateContentMap : BsonClassMap<TemporaryProblemUpdateContent>
    {
        public TemporaryProblemUpdateContentMap()
        {
            AutoMap();
            MapCreator(x => new TemporaryProblemUpdateContent(x.Title, x.Message));
        }
    }

    internal class TemporaryProblemSubscriberMap : BsonClassMap<TemporaryProblemSubscriber>
    {
        public TemporaryProblemSubscriberMap()
        {
            AutoMap();
        }
    }
}
