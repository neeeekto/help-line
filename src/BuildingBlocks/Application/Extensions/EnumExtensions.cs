using System.Collections.Generic;
using System.Linq;

namespace HelpLine.BuildingBlocks.Application.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> array, T item)
        {
            return array.Concat(new[] {item});
        }
    }
}
