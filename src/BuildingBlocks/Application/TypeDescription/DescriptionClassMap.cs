using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HelpLine.BuildingBlocks.Application.TypeDescription
{
    public abstract class DescriptionClassMap
    {
        private readonly Type _type;
        private List<DescriptionFieldMap> _fields = new();
        public string Key => _type.Name;
        public string? Title { get; internal set; }
        public string? Description { get; internal set; }
        public IEnumerable<DescriptionFieldMap> Fields => _fields.AsReadOnly();
        public IEnumerable<string>? Children { get; private set; }
        internal Description Ctx { get; set; }
        private

        protected DescriptionClassMap(Type type)
        {
            _type = type;
        }

        protected DescriptionFieldMap MapField<TType, TField>(Expression<Func<TType, TField>> getter)
        {

            var fieldInfo = DescriptionFieldMap.Make(getter, this);
            _fields.Add(fieldInfo);
            return fieldInfo;
        }

        protected void SetTitle(string title)
        {
            Title = title;
        }
        protected void SetDescription(string description)
        {
            Description = description;
        }

        protected void SetChildren(params Type[] children
        )
        {
            Children = children.Select(x => x.Name);
        }

        public abstract void Init();
    }

    public abstract class DescriptionClassMap<TType> : DescriptionClassMap
    {
        protected DescriptionClassMap() : base(typeof(TType))
        {
        }

        protected DescriptionFieldMap MapField<TField>(Expression<Func<TType, TField>> getter)
        {

            return MapField<TType, TField>(getter);
        }

    }
}
