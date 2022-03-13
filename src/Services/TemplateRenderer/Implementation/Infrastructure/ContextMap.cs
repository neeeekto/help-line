using HelpLine.Services.TemplateRenderer.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    internal class ContextMap : BsonClassMap<Context>
    {
        public ContextMap()
        {
            MapIdMember(x => x.Id);
            AutoMap();
            MapField(x => x.Data).SetSerializer(new JsonMongoSerializer());
        }
    }
}
