using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class EmailChannelSystemMap : BsonClassMap<EmailChannelSettings>
    {
        public EmailChannelSystemMap()
        {
            SetDiscriminator(nameof(EmailChannelSettings));
        }
    }
}
