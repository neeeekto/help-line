using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketsDelayConfigurationMap : BsonClassMap<TicketsDelayConfiguration>
    {
        public TicketsDelayConfigurationMap()
        {
            MapIdMember(x => x.ProjectId);
            AutoMap();
            MapMember(x => x.LifeCycleDelay)
                .SetSerializer(
                    new ReadOnlyDictionaryInterfaceImplementerSerializer<ReadOnlyDictionary<TicketLifeCycleType, TimeSpan>,
                        TicketLifeCycleType, TimeSpan>(DictionaryRepresentation.ArrayOfDocuments));
        }
    }
}
