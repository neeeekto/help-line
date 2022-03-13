using System.Reflection;

namespace HelpLine.Services.Files.Infrastructure
{
    internal static class ServiceInfo
    {
        public static string NameSpace = "Files";
        public static Assembly Assembly = typeof(ServiceInfo).Assembly;
    }
}
