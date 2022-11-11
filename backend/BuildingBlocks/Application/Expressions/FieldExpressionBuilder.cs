using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HelpLine.BuildingBlocks.Application.Expressions
{
    public static class FieldExpressionBuilder
    {
        public static Expression MakeGetter<T>(IEnumerable<string> path, out ParameterExpression pe)
        {
            var arrayFlag = false;
            pe = Expression.Parameter(typeof(T), "x");
            Expression getter = pe;
            foreach (var s in path)
            {
                if (arrayFlag)
                {
                    var indexExpr = Expression.ArrayIndex(getter, Expression.Constant(int.Parse(s)));
                    getter = indexExpr;
                }
                else
                {
                    MemberExpression member = Expression.Property(getter, s);
                    getter = member;
                }

                arrayFlag = getter.Type.IsArray;
            }

            return getter;
        }
    }
}
