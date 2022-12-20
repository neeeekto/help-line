using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search.Contracts;

namespace HelpLine.BuildingBlocks.Infrastructure.Search
{
    public abstract class AdditionalTypeProviderBase : IAdditionalTypeProvider
    {
        private readonly Dictionary<string, Type> _types;

        protected AdditionalTypeProviderBase()
        {
            _types = new Dictionary<string, Type>();
        }

        public Type Get(string key)
        {
            return _types[key];
        }

        protected void Add(string key, Type type)
        {
            _types.Add(key, type);
        }
    }
}
