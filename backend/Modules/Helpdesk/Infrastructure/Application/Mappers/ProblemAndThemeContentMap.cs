using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ProblemAndThemeContentMap : BsonClassMap<ProblemAndThemeContent>
    {
        public ProblemAndThemeContentMap()
        {
            AutoMap();
        }
    }
}
