using System.Reflection;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
        public static readonly Assembly Infrastructure = typeof(Assemblies).Assembly;
    }
}