namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models
{
    public abstract class ChannelSettings
    {
        private string _id => $"{ProjectId}:{Key}";
        public abstract string Key { get; }
        public string ProjectId { get; set; }
    }
}
