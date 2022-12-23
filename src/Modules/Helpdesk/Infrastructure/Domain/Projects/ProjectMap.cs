using HelpLine.Modules.Helpdesk.Domain.Projects;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Projects
{
    internal class ProjectMap : BsonClassMap<Project>
    {
        public ProjectMap()
        {
            MapIdMember(x => x.Id);
            AutoMap();
        }
    }

    internal class ProjectInfoMap : BsonClassMap<ProjectInfo>
    {
        public ProjectInfoMap()
        {
            AutoMap();
            MapCreator(x => new ProjectInfo(x.Name));
            MapCreator(x => new ProjectInfo(x.Name, x.Image));
        }
    }
}
