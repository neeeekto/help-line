namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class Tag
    {
        public string Key { get; internal set; }
        public string ProjectId { get; set; }
        public bool Enabled { get; internal set; }
    }
}
