using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Application.Expressions;
using HelpLine.BuildingBlocks.Application.Extensions;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.BuildingBlocks.Application.Search.Exception;
using MongoDB.Driver;
using zSpec.Automation;

namespace HelpLine.BuildingBlocks.Infrastructure.Search.Mongo
{
    // Good 
    internal class FilterBuilder<TModel>
    {
        private readonly IFilterValueGetter _filterValueGetter;
        private readonly IAdditionalTypeProvider _typeProvider;
        private readonly IValueMapper _valueMapper;

        public FilterBuilder(IFilterValueGetter filterValueGetter, IAdditionalTypeProvider typeProvider,
            IValueMapper valueMapper)
        {
            _filterValueGetter = filterValueGetter;
            _typeProvider = typeProvider;
            _valueMapper = valueMapper;
        }

        public FilterDefinition<TModel> Build(IFilter? filter, object ctx)
        {
            return Build<TModel>(filter, ctx);
        }

        public FilterDefinition<T> Build<T>(IFilter? filter, object ctx)
        {
            try
            {
                var builder = new FilterDefinitionBuilder<T>();
                var result = builder.Empty;
                switch (filter)
                {
                    case GroupFilter groupFilter:
                    {
                        result = Build<T>(groupFilter, ctx);
                        break;
                    }
                    case ValueFilter fieldFilter:
                    {
                        result = Build<T>(fieldFilter, ctx);
                        break;
                    }
                    case ElementMathFilter elementMathFilter:
                    {
                        result = Build<T>(elementMathFilter, ctx);
                        break;
                    }
                    case NotFilter notFilter:
                    {
                        result = Build<T>(notFilter, ctx);
                        break;
                    }
                    case OfTypeFilter ofTypeFilter:
                    {
                        result = Build<T>(ofTypeFilter, ctx);
                        break;
                    }
                    case ContainsFilter containsFilter:
                    {
                        result = Build<T>(containsFilter, ctx);
                        break;
                    }
                    case InFilter inFilter:
                    {
                        result = Build<T>(inFilter, ctx);
                        break;
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new FilterException(filter, e);
            }
        }

        private FilterDefinition<T> Build<T>(NotFilter filter, object ctx)
        {
            var builder = new FilterDefinitionBuilder<T>();
            return builder.Not(Build<T>(filter.Filter, ctx));
        }

        private FilterDefinition<T> Build<T>(ContainsFilter filter, object ctx)
        {
            var builder = new FilterDefinitionBuilder<T>();
            var fieldGetter = FieldExpressionBuilder.MakeGetter<T>(filter.Path, out var pe);
            var itemType = fieldGetter.Type.GetGenericArguments().First();
            var lambda = LamdaBuilder.MakeFuncExpression(typeof(T), fieldGetter.Type, fieldGetter, pe);

            var values = filter.Values.Select(x => GetValue(x, ctx, itemType)).ToArray().CastByType(itemType);

            var builderType = new FastTypeInfo(builder.GetType());
            var inType = builderType.PublicMethods
                .Where(x => x.Name == nameof(builder.AnyIn)
                            && x.GetParameters().Length == 2
                            && x.GetGenericArguments().Length == 1).Skip(1).First();
            var inFilter = inType.MakeGenericMethod(itemType);
            return ((FilterDefinition<T>) inFilter.Invoke(builder, new object[] {lambda, values}))!;
        }

        private FilterDefinition<T> Build<T>(GroupFilter filter, object ctx)
        {
            var builder = new FilterDefinitionBuilder<T>();
            return filter.Operation == GroupFilterOperators.Or
                ? builder.Or(filter.Filters.Select((f) => Build<T>(f, ctx)))
                : builder.And(filter.Filters.Select((f) => Build<T>(f, ctx)));
        }

        private FilterDefinition<T> Build<T>(InFilter filter, object ctx)
        {
            var builder = new FilterDefinitionBuilder<T>();
            var fieldGetter = FieldExpressionBuilder.MakeGetter<T>(filter.Path, out var pe);

            var builderType = new FastTypeInfo(builder.GetType());
            var inType = builderType.PublicMethods
                .Where(x => x.Name == nameof(builder.In)
                            && x.GetParameters().Length == 2
                            && x.GetGenericArguments().Length == 1)
                .Skip(1)
                .First();
            var inFn = inType.MakeGenericMethod(fieldGetter.Type);
            var values = filter.Values.Select(x => GetValue(x, ctx, fieldGetter.Type)).ToArray()
                .CastByType(fieldGetter.Type);

            var getterExp = LamdaBuilder.MakeFuncExpression(typeof(T), fieldGetter.Type, fieldGetter, pe);
            return (FilterDefinition<T>) inFn.Invoke(builder, new object?[] {getterExp, values})!;
        }

        private FilterDefinition<T> Build<T>(OfTypeFilter filter, object ctx)
        {
            var builder = new FilterDefinitionBuilder<T>();
            var needType = _typeProvider.Get(filter.Type);
            if (IsSimpleType(needType))
                throw new ArgumentException("OfTypeFilter cannot apply for primitive value!");

            var builderType = new FastTypeInfo(builder.GetType());
            var ofTypeType = builderType.PublicMethods
                .First(x => x.Name == nameof(builder.OfType)
                            && x.GetParameters().Length == 1
                            && x.GetGenericArguments().Length == 1);
            var ofType = ofTypeType.MakeGenericMethod(needType);

            var thisType = new FastTypeInfo(GetType());
            var makeMethodType =
                thisType.PublicMethods.First(x => x.Name == nameof(Build) && x.ContainsGenericParameters);
            var makeMethod = makeMethodType.MakeGenericMethod(needType);
            var itemFilter = makeMethod.Invoke(this, new[] {filter.Filter, ctx});
            return (FilterDefinition<T>) ofType.Invoke(builder, new[] {itemFilter});
        }

        private FilterDefinition<T> Build<T>(ValueFilter filter, object ctx)
        {
            var builder = new FilterDefinitionBuilder<T>();
            var fieldGetter = FieldExpressionBuilder.MakeGetter<T>(filter.Path, out var pe);
            var value = GetValue(filter.Value, ctx, fieldGetter.Type);
            var valueExpression = Expression.Convert(Expression.Constant(value), fieldGetter.Type);
            Expression ex = Expression.Equal(fieldGetter, valueExpression);

            switch (filter.Operator)
            {
                case FieldFilterOperators.Equal:
                    ex = Expression.Equal(fieldGetter, valueExpression);
                    break;
                case FieldFilterOperators.Great:
                    ex = Expression.GreaterThan(fieldGetter, valueExpression);
                    break;
                case FieldFilterOperators.GreatOrEqual:
                    ex = Expression.GreaterThanOrEqual(fieldGetter, valueExpression);
                    break;
                case FieldFilterOperators.Less:
                    ex = Expression.LessThan(fieldGetter, valueExpression);
                    break;
                case FieldFilterOperators.LessOrEqual:
                    ex = Expression.LessThanOrEqual(fieldGetter, valueExpression);
                    break;
                case FieldFilterOperators.NotEqual:
                    ex = Expression.NotEqual(fieldGetter, valueExpression);
                    break;
            }

            Expression<Func<T, bool>> where = Expression.Lambda<Func<T, bool>>(ex, pe);
            return builder.Where(where);
        }

        private FilterDefinition<T> Build<T>(ElementMathFilter mathFilter, object ctx)
        {
            // TODO: Это работает, не выеб@йся. Отрефачить и кэш докинуть если варик
            var builder = new FilterDefinitionBuilder<T>();
            var fieldGetter = FieldExpressionBuilder.MakeGetter<T>(mathFilter.Path, out var pe);
            bool isDict = fieldGetter.Type.IsGenericType && (
                fieldGetter.Type.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                || fieldGetter.Type.GetGenericTypeDefinition() == typeof(Dictionary<,>)
                || fieldGetter.Type.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)
            );
            var itemType = fieldGetter.Type.GetGenericArguments().First();
            if (isDict)
            {
                itemType = typeof(KeyValuePair<,>).MakeGenericType(fieldGetter.Type.GetGenericArguments());
            }

            if (IsSimpleType(itemType))
                throw new ArgumentException("ArrayMathFilter not supported for primitive value, use ValueFilter!");

            var builderType = new FastTypeInfo(builder.GetType());
            var elemMatchType = builderType.PublicMethods
                .Where(x => x.Name == nameof(builder.ElemMatch) && x.GetParameters().Length == 2)
                .Skip(1) // TODO: Skip 1 - ахуеть не встать
                .First();
            var elemMatch = elemMatchType.MakeGenericMethod(itemType);

            var thisType = new FastTypeInfo(GetType());
            var makeMethodType =
                thisType.PublicMethods.First(x => x.Name == nameof(Build) && x.ContainsGenericParameters);
            var makeMethod = makeMethodType.MakeGenericMethod(itemType);

            // TODO: Replace to LambdaBuilder
            var lambda = FastTypeInfo<Expression>.PublicMethods.First(x => x.Name == nameof(Expression.Lambda));
            var resType = typeof(Func<,>).MakeGenericType(typeof(T), typeof(IEnumerable<>).MakeGenericType(itemType));
            lambda = lambda.MakeGenericMethod(resType);
            var expr = lambda.Invoke(null, new object[] {fieldGetter, new[] {pe}});

            var itemFilter = makeMethod.Invoke(this, new[] {mathFilter.ItemFilter, ctx});
            return (FilterDefinition<T>) elemMatch.Invoke(builder, new[] {expr, itemFilter});
        }

        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                   || type.Equals(typeof(string));
        }

        private object GetValue(FilterValue filterValue, object ctx, Type awaitType)
        {
            var value = _filterValueGetter.Get(filterValue, ctx);
            if (awaitType != value.GetType() &&
                _valueMapper.TryMap(awaitType, value, out var mappedValue))
                value = mappedValue;
            return value;
        }
    }
}
