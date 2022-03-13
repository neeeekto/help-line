using HelpLine.Services.TemplateRenderer.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    internal class ComponentMap : BsonClassMap<Component>
    {
        public ComponentMap()
        {
            MapIdMember(x => x.Id);
            AutoMap();
        }
    }
}
