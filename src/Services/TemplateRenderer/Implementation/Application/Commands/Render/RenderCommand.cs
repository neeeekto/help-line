using System.Collections.Generic;
using HelpLine.Services.TemplateRenderer.Contracts;
using MediatR;

namespace HelpLine.Services.TemplateRenderer.Application.Commands.Render
{
    public class RenderCommand : ICommand<IDictionary<string, string>>
    {
        public IEnumerable<string> TemplatesIds { get; }
        public object Data { get; }

        public RenderCommand(IEnumerable<string> templatesIds, object data)
        {
            TemplatesIds = templatesIds;
            Data = data;
        }
    }
}
