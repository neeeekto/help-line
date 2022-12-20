using HelpLine.Modules.Helpdesk.Application.TemporaryProblems;
using HelpLine.Modules.Helpdesk.Application.TemporaryProblems.DTO;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.TemporaryProblems
{
    internal class TemporaryProblemViewMap : BsonClassMap<TemporaryProblemDto>
    {
        public TemporaryProblemViewMap()
        {
            MapIdField(x => x.Id);
            AutoMap();
        }
    }

    internal class TemporaryProblemInfoViewMap : BsonClassMap<TemporaryProblemInfoDto>
    {
        public TemporaryProblemInfoViewMap()
        {
            AutoMap();
        }
    }

    internal class TemporaryProblemMetaViewMap : BsonClassMap<TemporaryProblemMetaDto>
    {
        public TemporaryProblemMetaViewMap()
        {
            AutoMap();
        }
    }

    internal class TemporaryProblemUpdateViewMap : BsonClassMap<TemporaryProblemUpdateDto>
    {
        public TemporaryProblemUpdateViewMap()
        {
            AutoMap();
        }
    }
    internal class TemporaryProblemUpdateContentViewMap : BsonClassMap<TemporaryProblemUpdateContentDto>
    {
        public TemporaryProblemUpdateContentViewMap()
        {
            AutoMap();
        }
    }

    internal class TemporaryProblemSubscriberViewMap : BsonClassMap<TemporaryProblemSubscriberDto>
    {
        public TemporaryProblemSubscriberViewMap()
        {
            AutoMap();
        }
    }
}
