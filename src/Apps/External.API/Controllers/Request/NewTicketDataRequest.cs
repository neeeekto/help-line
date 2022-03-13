using System.Collections.Generic;

namespace HelpLine.Apps.External.API.Controllers.Request
{

    public class ChannelItem
    {
        public string UserId { get; set; }
        public string Channel { get; set; }
    }
    public class NewTicketDataRequest
    {
        public string Language { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<ChannelItem> Channels { get; set; } // {userId: channelKey}
        public IEnumerable<KeyValuePair<string, string>> UserMeta { get; set; } // {key: val} es: {device: phone, os: android 8.1}
        public string Text { get; set; }
        public IEnumerable<string>? Attachments { get; set; }
        public string? FromTicket { get; set; }
        public string Source { get; set; }
        public string? Platform { get; set; }
    }
}
