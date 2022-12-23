using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HelpLine.BuildingBlocks.Application.Expressions;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Application.Search.Exception;
using MongoDB.Driver;
using zSpec.Automation;

namespace HelpLine.BuildingBlocks.Infrastructure.Search.Mongo
{
    internal class SortBuilder<TModel>
    {
        public SortBuilder()
        {
        }

        public SortDefinition<TModel> Build(IEnumerable<Sort> sorts)
        {
            var builder = new SortDefinitionBuilder<TModel>();
            return builder.Combine(sorts.Select(Build));
        }

        private SortDefinition<TModel> Build(Sort sort)
        {
            try
            {
                var builder = new SortDefinitionBuilder<TModel>();
                var fieldGetter = FieldExpressionBuilder.MakeGetter<TModel>(sort.Path, out var pe);
                Expression<Func<TModel, object>> lambda =
                    Expression.Lambda<Func<TModel, object>>(Expression.Convert(fieldGetter, typeof(object)), pe);
                return sort.Asc ? builder.Ascending(lambda) : builder.Descending(lambda);
            }
            catch (Exception e)
            {
                throw new SortException(sort, e);
            }
        }
    }
}
