using System;
using HelpLine.BuildingBlocks.Application.Search.Contracts;

namespace HelpLine.BuildingBlocks.Infrastructure.Search
{
    public class NoopValueMapper : IValueMapper
    {
        public bool TryMap(Type target, object value, out object result)
        {
            result = value;
            return false;
        }
    }
}
