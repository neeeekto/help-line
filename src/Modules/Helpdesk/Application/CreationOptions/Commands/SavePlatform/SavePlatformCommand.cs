using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform
{
    public class SavePlatformCommand : CommandBase
    {
        public string Key { get; }
        public string ProjectId { get; }
        public string Name { get; }
        public string? Icon { get; }
        public int? Sort { get; }


        public SavePlatformCommand(string key, string projectId, string name, string? icon, int? sort = null)
        {
            Key = key;
            ProjectId = projectId;
            Name = name;
            Icon = icon;
            Sort = sort;
        }
    }
}
