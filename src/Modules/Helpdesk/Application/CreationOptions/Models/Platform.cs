
namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Models
{
    public class Platform
    {
        private string _id => $"{ProjectId}:{Key}"; // only for mongo
        public string Key { get; internal set; }
        public string Name { get; internal set; } // View Name for users
        public string ProjectId { get; internal set; }
        public int Sort { get; internal set; }
        public string? Icon { get; internal set; }

    }
}
