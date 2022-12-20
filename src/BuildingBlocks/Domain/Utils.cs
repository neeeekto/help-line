using System.Collections.Generic;
using System.Linq;

namespace HelpLine.BuildingBlocks.Domain
{
    public static class Utils
    {
        public static IDictionary<TKey, TVal> MapToDictionary<TKey, TVal>(this IReadOnlyDictionary<TKey, TVal> rd)
        {
            return rd.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}