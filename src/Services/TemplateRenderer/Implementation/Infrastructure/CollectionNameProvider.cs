using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Services.TemplateRenderer.Models;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    internal class CollectionNameProvider : BuildingBlocks.Infrastructure.Data.CollectionNameProvider
    {
        public CollectionNameProvider() : base(ServiceInfo.NameSpace)
        {
            Add<Component>("Components");
            Add<Context>("Contexts");
            Add<Template>("Templates");
        }
    }
}
