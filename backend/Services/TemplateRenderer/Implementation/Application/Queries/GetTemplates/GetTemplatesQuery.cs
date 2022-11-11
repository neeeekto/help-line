using System.Collections.Generic;
using HelpLine.Services.TemplateRenderer.Contracts;
using HelpLine.Services.TemplateRenderer.Models;

namespace HelpLine.Services.TemplateRenderer.Application.Queries.GetTemplates
{
    public class GetTemplatesQuery : ICommand<IEnumerable<Template>>
    {

    }
}
