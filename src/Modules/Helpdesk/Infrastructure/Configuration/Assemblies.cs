using System.Reflection;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
        public static readonly Assembly Infrastructure = typeof(Assemblies).Assembly;
    }
}
