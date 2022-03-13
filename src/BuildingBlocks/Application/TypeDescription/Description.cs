using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpLine.BuildingBlocks.Application.TypeDescription
{
    public class Description
    {
        private List<DescriptionEnum> _enums = new();

        public string Root { get; }
        public IEnumerable<DescriptionClassMap> Types;
        public IEnumerable<DescriptionEnum> Enums => _enums.AsReadOnly();

        public Description(IEnumerable<DescriptionClassMap> maps, Type rootType)
        {
            Root = rootType.Name;
            Types = maps.ToList();

            foreach (var cm in Types)
            {
                cm.Ctx = this;
                cm.Init();
            }
        }

        internal DescriptionEnum AddEnum(Type enumType)
        {
            var exist = _enums.Find(x => x.Key == enumType.Name);
            if (exist is not null)
                return exist;
            var keys = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);
            var enumValues = new Dictionary<string, int>();
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var value = (int)values.GetValue(i)!;
                enumValues.Add(key, value);
            }

            var result = new DescriptionEnum(enumType.Name, enumValues);
            _enums.Add(result);
            return result;
        }
    }
}
