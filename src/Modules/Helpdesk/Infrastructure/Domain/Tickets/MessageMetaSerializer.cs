using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class MessageMetaSerializer : ReadOnlyDictionaryInterfaceImplementerSerializer<MessageMeta, string, string>
{

}