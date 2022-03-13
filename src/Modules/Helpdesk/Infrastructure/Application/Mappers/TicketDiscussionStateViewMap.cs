using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketDiscussionStateViewMap : BsonClassMap<TicketDiscussionStateView>
    {
        public TicketDiscussionStateViewMap()
        {
            AutoMap();
            MapMember(x => x.IterationCount);
            MapMember(x => x.LastReplyDate).SetIgnoreIfNull(true);
        }
    }
}