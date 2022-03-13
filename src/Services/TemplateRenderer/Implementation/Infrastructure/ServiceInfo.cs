using System.Reflection;

namespace HelpLine.Services.TemplateRenderer.Infrastructure
{
    internal static class ServiceInfo
    {
        public static string NameSpace = "TemplateRenderer";
        public static Assembly Assembly = typeof(ServiceInfo).Assembly;
    }
}
