using System.Reflection;

namespace HelpLine.Services.Migrations.Infrastructure
{
    internal static class ServiceInfo
    {
        public static string NameSpace = "Migrations";
        public static Assembly Assembly = typeof(ServiceInfo).Assembly;
    }
}
