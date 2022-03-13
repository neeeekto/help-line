using System;
using HelpLine.Apps.Client.API.Configuration.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Configuration.Utils
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectParamAttribute : FromHeaderAttribute
    {
        public ProjectParamAttribute()
        {
            Name = ProjectMiddleware.ProjectHeaderKey;
        }
    }
}
