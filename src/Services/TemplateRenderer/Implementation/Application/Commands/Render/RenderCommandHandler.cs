using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HandlebarsDotNet;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using HelpLine.Services.TemplateRenderer.Models;
using MediatR;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Context = HelpLine.Services.TemplateRenderer.Models.Context;

namespace HelpLine.Services.TemplateRenderer.Application.Commands.Render
{
    internal class RenderCommandHandler : IRequestHandler<RenderCommand, IDictionary<string, string>>
    {
        private readonly MongoContext _context;

        public RenderCommandHandler(MongoContext context)
        {
            _context = context;
        }

        public async Task<IDictionary<string, string>> Handle(RenderCommand request, CancellationToken cancellationToken)
        {
            var templates = await _context.Templates.Find(x => request.TemplatesIds.Contains(x.Id) )
                .ToListAsync(cancellationToken: cancellationToken);
            if (!templates.Any())
                throw new NotFoundException(request.TemplatesIds);

            var handlebars = Handlebars.Create();
            await LoadUtilities(handlebars);
            await LoadComponents(handlebars, cancellationToken);


            var data = JObject.FromObject(request.Data);
            var result = new Dictionary<string, string>();
            var contexts = await _context.Contexts.Find(x => true)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (var template in templates)
            {
                var renderCtx = new JObject
                {
                    ["ctx"] = await LoadContexts(template, contexts, cancellationToken),
                    ["props"] = LoadProps(template),
                    ["data"] = data
                };
                var renderer = handlebars.Compile(template.Content);
                result.Add(template.Id, renderer(renderCtx));
            }

            return result;
        }

        private async Task LoadComponents(IHandlebars handlebars,
            CancellationToken cancellationToken)
        {
            var components = await _context.Components.Find(x => true)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (var component in components)
                handlebars.RegisterTemplate(component.Id, component.Content);
        }

        private async Task<JObject> LoadContexts(Template template, IEnumerable<Context> contexts,
            CancellationToken cancellationToken)
        {

            var result = new JObject();
            var templateContexts = contexts.Where(x => template.Contexts.Contains(x.Id));
            foreach (var context in templateContexts)
            {
                var contextData = JObject.FromObject(context.Data);
                if (context.Extend != null)
                {
                    var extendContext = contexts.FirstOrDefault(x => x.Id == context.Extend);
                    if (extendContext != null)
                    {
                        var extendData = JObject.FromObject(extendContext.Data);
                        extendData.Merge(contextData, new JsonMergeSettings()
                        {
                            MergeArrayHandling = MergeArrayHandling.Concat,
                            MergeNullValueHandling = MergeNullValueHandling.Ignore
                        });
                        contextData = extendData;
                    }
                }
                result[context.Id] = contextData;
                if (context.Alias != null)
                {
                    result[context.Alias] = contextData;
                }
            }

            return result;
        }

        private JObject LoadProps(Template template)
        {
            return JObject.FromObject(template.Props);
        }

        private async Task LoadUtilities(IHandlebars handlebars)
        {
        }
    }
}
