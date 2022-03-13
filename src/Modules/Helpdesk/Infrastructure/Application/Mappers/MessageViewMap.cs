using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class MessageViewMap : BsonClassMap<MessageView>
    {
        public MessageViewMap()
        {
            AutoMap();
            MapMember(x => x.Attachments).SetDefaultValue(new List<string>());
            MapMember(x => x.Text);
        }
    }
}