using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TagsDescriptionIssueContentMap : BsonClassMap<TagsDescriptionIssueContent>
    {
        public TagsDescriptionIssueContentMap()
        {
            AutoMap();
        }
    }
}
