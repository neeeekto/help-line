using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.TypeDescription
{
    public class DescriptionEnum
    {
        public string Key { get; }
        public IReadOnlyDictionary<string, int> Values { get; }

        public DescriptionEnum(string key, Dictionary<string, int> values)
        {
            Key = key;
            Values = values;
        }
    }
}
