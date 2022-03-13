using System;

namespace HelpLine.Apps.Client.API.Configuration.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NoPermissionRequiredAttribute : Attribute
    {
    }
}
