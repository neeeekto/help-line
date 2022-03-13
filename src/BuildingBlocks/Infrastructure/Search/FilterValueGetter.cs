using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Application.Expressions;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Application.Search.Contracts;

namespace HelpLine.BuildingBlocks.Infrastructure.Search
{
    public class FilterValueGetter<TContext> : IFilterValueGetter
    {
        private readonly ConcurrentDictionary<string, Delegate> _cache =
            new ConcurrentDictionary<string, Delegate>();

        public object Get(FilterValue value, object ctx)
        {
            switch (value)
            {
                case ConstantFilterValue cnstValue:
                    return cnstValue.Value;
                case ContextFilterValue ctxValue:
                    return GetByContext(ctxValue, ctx);
                default:
                    throw new ArgumentException("Unknown FilterValue type");
            }
        }

        private object GetByContext(ContextFilterValue ctxValue, object ctx)
        {
            var key = string.Join(".", ctxValue.Path);
            var getter = _cache.GetOrAdd(key, MakeGetter(ctxValue.Path));
            return getter.DynamicInvoke(ctx);
        }

        private Delegate MakeGetter(IEnumerable<string> path)
        {
            var getter = FieldExpressionBuilder.MakeGetter<TContext>(path, out var pe);
            var getterExp = LamdaBuilder.MakeFuncExpression(typeof(TContext), getter.Type, getter, pe);
            return getterExp.Compile();
        }
    }
}
