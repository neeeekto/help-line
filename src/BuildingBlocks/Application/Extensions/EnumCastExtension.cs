using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HelpLine.BuildingBlocks.Application.Extensions
{
    public static class EnumCastExtension
    {
        private static MethodInfo Cast = typeof(System.Linq.Enumerable)
            .GetMethod("Cast", BindingFlags.Static | BindingFlags.Public)!;
        public static Array CastByType(this IEnumerable<object> array, Type toType)
        {
            var values = (IEnumerable) Cast.MakeGenericMethod(toType)
                .Invoke(null, new object[] {array})!;

            ArrayList list = new ArrayList(); // Because "values" is lazy enum
            foreach (var value in values)
            {
                list.Add(value);
            }

            return list.ToArray(toType);
        }
    }
}
