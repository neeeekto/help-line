using System;

namespace HelpLine.Apps.Client.API.Configuration.PermissionInfo
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PermissionsDependOnAttribute : Attribute
    {
        public PermissionsDependOnAttribute(string parent)
        {
            Parent = parent;
        }

        public string Parent { get; }
    }
}
