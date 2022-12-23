using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HelpLine.Apps.Client.API.Configuration.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    internal class HasPermissionsAttribute : AuthorizeAttribute
    {
        internal static string HasPermissionPolicyName = "HasPermission";
        public IEnumerable<string> Permissions { get; }
        public InclusionType Inclusion { get; }


        public HasPermissionsAttribute(IEnumerable<string> permissions, InclusionType inclusionType = InclusionType.And) : base(HasPermissionPolicyName)
        {
            Permissions = permissions;
            Inclusion = inclusionType;
        }

        public HasPermissionsAttribute(string name) : base(HasPermissionPolicyName)
        {
            Permissions = new []{name};
            Inclusion = InclusionType.And;
        }


        internal enum InclusionType
        {
            Or,
            And
        }
    }
}
