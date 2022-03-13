using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Contracts;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork
{
    public class TemplateRendererMock : ITemplateRenderer
    {

        public Task<string> Render(string templateId, object data)
        {
            throw new System.NotImplementedException();
        }

        public Task<IDictionary<string, string>> Render(IEnumerable<string> templatesIds, object data)
        {
            throw new System.NotImplementedException();
        }
    }
}
