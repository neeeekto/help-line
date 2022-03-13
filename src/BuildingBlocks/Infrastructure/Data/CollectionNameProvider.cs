using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public abstract class CollectionNameProvider : ICollectionNameProvider
    {
        private readonly Dictionary<Type, string> _names;
        private readonly string _nameSpace;

        protected CollectionNameProvider(string nameSpace)
        {
            _names = new Dictionary<Type, string>();
            _nameSpace = nameSpace;
        }

        protected void Add<T>(string name)
        {
            var type = typeof(T);
            var fullName = string.IsNullOrEmpty(_nameSpace) ? name : $"{_nameSpace}.{name}";
            var exc = new ArgumentException($"Collection already exist");
            if(_names.ContainsKey(type)) throw exc;
            if(_names.ContainsValue(fullName)) throw exc;
            _names.Add(typeof(T), fullName);
        }

        public string Get<T>()
        {
            if (_names.TryGetValue(typeof(T), out var name))
            {
                return name;
            }

            throw new ApplicationException($"Collection name for {typeof(T).FullName} not found");
        }

        public IEnumerable<string> All()
        {
            return _names.Select(x => x.Value);
        }
    }
}
