using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Contracts;
using HelpLine.Services.TemplateRenderer.Application.Commands.Render;

namespace HelpLine.Services.TemplateRenderer
{
    public class TemplateRenderer : ITemplateRenderer
    {
        public async Task<string> Render(string templateId, object data)
        {
            var result =
                await TemplateRendererService.InternalExecuteAsync(new RenderCommand(new[] {templateId}, data));
            return result[templateId];
        }

        public Task<IDictionary<string, string>> Render(IEnumerable<string> templatesIds, object data)
        {
            return TemplateRendererService.InternalExecuteAsync(new RenderCommand(templatesIds, data));
        }
    }
}
