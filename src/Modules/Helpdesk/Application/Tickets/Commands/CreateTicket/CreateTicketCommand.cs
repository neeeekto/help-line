using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket
{
    public class CreateTicketCommand : CommandBase<string>
    {
        public string Project { get; }
        public string Language { get; }
        public InitiatorDto Initiator { get; }
        public IEnumerable<string> Tags { get; }
        public IDictionary<string, string> Channels { get; } // {userId: channelKey}
        public IDictionary<string, string> UserMeta { get; } // {key: val} es: {device: phone, os: android 8.1}
        public MessageDto? Message { get; }
        public string? FromTicket { get; }
        public string? Platform { get; }
        public string Source { get; }

        public CreateTicketCommand(string project, string language, InitiatorDto initiator, IEnumerable<string> tags,
            IDictionary<string, string> channels, IDictionary<string, string> userMeta, string? fromTicket,
            MessageDto? message, string source, string? platform)
        {
            Project = project;
            Language = language;
            Initiator = initiator;
            Tags = tags;
            Channels = channels;
            UserMeta = userMeta;
            FromTicket = fromTicket;
            Message = message;
            Source = source;
            Platform = platform;
        }
    }
}
