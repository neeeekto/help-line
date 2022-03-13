using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace HelpLine.Apps.Client.API.Configuration.Authorization
{
    public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute>
        : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
        where TAttribute : Attribute
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var httpContext = context.Resource as HttpContext;
            var attributes = httpContext.GetEndpoint().Metadata.OfType<TAttribute>();

            return HandleRequirementAsync(context, requirement, attributes);
        }


        protected abstract Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TRequirement requirement,
            IEnumerable<TAttribute> attribute);
    }
}
