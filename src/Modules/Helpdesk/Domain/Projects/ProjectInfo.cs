using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Projects
{
    public class ProjectInfo : ValueObject
    {
        public string Name { get; }
        public string? Image { get; }

        public ProjectInfo(string name, string image)
        {
            Name = name;
            Image = image;
        }

        public ProjectInfo(string name)
        {
            Name = name;
            Image = null;
        }
    }
}
