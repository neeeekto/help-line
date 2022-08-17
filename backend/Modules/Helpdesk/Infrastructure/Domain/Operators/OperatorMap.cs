using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson.Serialization;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Operators
{
    internal class OperatorMap : BsonClassMap<Operator>
    {
        public OperatorMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
            MapMember(x => x.Roles).SetSerializer(
                new ReadOnlyDictionaryInterfaceImplementerSerializer<ReadOnlyDictionary<string, IEnumerable<Guid>>,
                    string, IEnumerable<Guid>>(DictionaryRepresentation.ArrayOfDocuments)).SetDefaultValue(
                new ReadOnlyDictionary<string, IEnumerable<Guid>>(new Dictionary<string, IEnumerable<Guid>>()));
        }
    }
}
