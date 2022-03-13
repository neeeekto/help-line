using System;

namespace HelpLine.Apps.Client.API.Configuration.PermissionInfo
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PermissionNameAttribute : Attribute
    {
        public readonly string Name;

        public PermissionNameAttribute(string name)
        {
            Name = name;
        }
    }
}
