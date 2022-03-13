using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketViewMap : BsonClassMap<TicketView>
    {
        public TicketViewMap()
        {
            MapIdMember(x => x.Id);
            MapMember(x => x.Events);
            MapMember(x => x.Feedbacks);
            MapMember(x => x.Language);
            MapMember(x => x.Priority);
            MapMember(x => x.Status);
            MapMember(x => x.Tags);
            MapMember(x => x.Title);
            MapMember(x => x.AssignedTo);
            MapMember(x => x.CreateDate);
            MapMember(x => x.DiscussionState);
            MapMember(x => x.HardAssigment);
            MapMember(x => x.ProjectId);
            MapMember(x => x.UserIds);
            MapMember(x => x.UserMeta)
                .SetSerializer(
                    new DictionaryInterfaceImplementerSerializer<Dictionary<string, string>, string, string>(
                        DictionaryRepresentation.ArrayOfDocuments)); // only for search provider
            MapMember(x => x.DateOfLastStatusChange);
        }
    }
}
