using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets
{
    internal class TicketEventMongoMap : BsonClassMap<TicketEventBase>
    {
        public TicketEventMongoMap()
        {
            AutoMap();
            MapMember(x => x.Initiator);
            MapMember(x => x.CreateDate);
        }
    }
}
