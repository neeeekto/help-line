using System;
using System.Linq;
using System.Linq.Expressions;
using zSpec.Automation;

namespace HelpLine.BuildingBlocks.Application.Expressions
{
    public static class LamdaBuilder
    {
        public static LambdaExpression MakeFuncExpression(Type inType, Type outType, Expression bodyExpression, ParameterExpression parameterExpression)
        {
            var lambda = FastTypeInfo<Expression>.PublicMethods.First(x => x.Name == nameof(Expression.Lambda));
            var resType = typeof(Func<,>).MakeGenericType(inType, outType);
            lambda = lambda.MakeGenericMethod(resType);
            return (LambdaExpression)lambda.Invoke(null, new object[] {bodyExpression, new[] {parameterExpression}})!;
        }
    }
}
