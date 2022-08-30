using System;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Configuration.Utils;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class VersionParamAttribute : FromHeaderAttribute
{
    public VersionParamAttribute()
    {
        Name = "ETag";
    }
}
