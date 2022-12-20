using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TagsDescriptionIssueMap : BsonClassMap<TagsDescriptionIssue>
    {
        public TagsDescriptionIssueMap()
        {
            AutoMap();
        }
    }
}
