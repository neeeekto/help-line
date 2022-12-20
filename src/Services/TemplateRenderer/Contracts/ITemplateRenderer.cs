using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.Services.TemplateRenderer.Contracts
{
    public interface ITemplateRenderer
    {
        Task<string> Render(string templateId, object data);
        Task<IDictionary<string, string>> Render(IEnumerable<string> templatesIds, object data);
    }
}
