using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HelpLine.BuildingBlocks.Application.TypeDescription
{
    public class DescriptionFieldMap
    {
        public IEnumerable<string> Path { get; private set; }
        public string Name { get; internal set; }
        public string? Description { get; internal set; }
        public DescriptionFieldType Type { get; private set; }
        public bool Optional { get; private set; }

        private readonly PropertyInfo _propertyInfo;
        private DescriptionClassMap _classMap;

        internal static DescriptionFieldMap Make<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertyLambda, DescriptionClassMap classMap)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a field, not a property.");

            var visitor = new PropertyVisitor();
            visitor.Visit(propertyLambda.Body);
            visitor.Path.Reverse();
            return new DescriptionFieldMap(propInfo, visitor.Path.Select(x => x.Name), classMap);
        }

        private DescriptionFieldMap(PropertyInfo propertyInfo, IEnumerable<string> path, DescriptionClassMap classMap)
        {
            _propertyInfo = propertyInfo;
            Path = path;
            _classMap = classMap;
            Optional = false;
            Name = propertyInfo.Name;
            Type = GetDescriptionType(propertyInfo.PropertyType);
        }

        private DescriptionFieldType GetDescriptionType(Type type)
        {
            if (type.IsEnum)
            {
                var enumDesc = _classMap.Ctx.AddEnum(type);
                return new EnumDescriptionFieldType(enumDesc.Key);
            }

            if (type.IsPrimitive)
            {
                var primitiveType = PrimitiveDescriptionFieldType.Primitives.Number;
                if (type == typeof(int))
                    primitiveType = PrimitiveDescriptionFieldType.Primitives.Number;
                if (type == typeof(bool))
                    primitiveType = PrimitiveDescriptionFieldType.Primitives.Boolean;
                return new PrimitiveDescriptionFieldType(primitiveType);
            }

            if (type == typeof(string))
            {
                return new PrimitiveDescriptionFieldType(PrimitiveDescriptionFieldType.Primitives.String);
            }

            if (type == typeof(DateTime))
            {
                return new PrimitiveDescriptionFieldType(PrimitiveDescriptionFieldType.Primitives.Date);
            }

            if (type == typeof(Guid))
            {
                return new PrimitiveDescriptionFieldType(PrimitiveDescriptionFieldType.Primitives.String);
            }

            if (Nullable.GetUnderlyingType(type) is not null)
            {
                Optional = true;
                return GetDescriptionType(Nullable.GetUnderlyingType(type));
            }

            if (type.IsArray)
            {
                return new ArrayDescriptionFieldType(GetDescriptionType(type.GetElementType()));
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type keyType = type.GetGenericArguments()[0];
                Type valueType = type.GetGenericArguments()[1];
                return new DictionaryDescriptionFieldType(GetDescriptionType(keyType), GetDescriptionType(valueType));
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return new ArrayDescriptionFieldType(GetDescriptionType(type.GetGenericArguments()[0]));
            }



            return new ClassDescriptionFieldType(type.Name);
        }

        private class PropertyVisitor : ExpressionVisitor
        {
            internal readonly List<MemberInfo> Path = new List<MemberInfo>();

            protected override Expression VisitMember(MemberExpression node)
            {
                if (!(node.Member is PropertyInfo))
                {
                    throw new ArgumentException("The path can only contain properties", nameof(node));
                }

                Path.Add(node.Member);
                return base.VisitMember(node);
            }
        }

        public DescriptionFieldMap SetName(string name)
        {
            Name = name;
            return this;
        }

        public DescriptionFieldMap SetDescription(string description)
        {
            Description = description;
            return this;
        }
    }
}
