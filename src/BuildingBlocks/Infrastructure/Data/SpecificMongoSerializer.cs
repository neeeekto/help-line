using System;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SpecificMongoSerializer : Attribute
    {

    }
}
