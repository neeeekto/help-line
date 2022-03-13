using HelpLine.Services.TemplateRenderer.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    internal class TemplateMap : BsonClassMap<Template>
    {
        public TemplateMap()
        {
            MapIdMember(x => x.Id);
            AutoMap();
            MapField(x => x.Props).SetSerializer(new JsonMongoSerializer());
        }
    }
}
