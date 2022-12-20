using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class FeedbackReviewViewMap : BsonClassMap<FeedbackReviewView>
    {
        public FeedbackReviewViewMap()
        {
            AutoMap();
            MapMember(x => x.FeedbackId);
            MapMember(x => x.Message).SetIgnoreIfNull(true);
            MapMember(x => x.Score);
            MapMember(x => x.Solved).SetIgnoreIfNull(true);
            MapMember(x => x.OptionalScores).SetIgnoreIfNull(true);
        }
    }
}