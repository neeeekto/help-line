using System;

namespace HelpLine.Apps.Client.API.Features.System.Models
{
    public class MessageData
    {
        public string Text { get; set; }
        public MessageLvl Lvl { get; set; }
        public DateTime? ShowAt { get; set; }
        public DateTime? HideAt { get; set; }
        public DateTime? WillHappenAt { get; set; }
    }
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public MessageData Data { get; set; }
    }

    public enum MessageLvl
    {
        Info,
        Warning,
        Danger
    }
}
