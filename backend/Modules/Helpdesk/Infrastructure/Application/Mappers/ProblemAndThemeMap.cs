using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ProblemAndThemeMap : BsonClassMap<ProblemAndThemeRoot>
    {
        public ProblemAndThemeMap()
        {
            MapIdProperty("_id");
            AutoMap();
        }
    }
}
