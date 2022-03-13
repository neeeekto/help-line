using System;

namespace HelpLine.BuildingBlocks.Application.Search.Contracts
{
    public interface IValueMapper
    {
        public bool TryMap(Type target, object value, out object result);
    }
}
