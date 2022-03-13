using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HelpLine.BuildingBlocks.Application.Utils
{
    public class InstanceCreator
    {
        private readonly Dictionary<Type, Delegate> _cache;


        public InstanceCreator()
        {
            _cache = new Dictionary<Type, Delegate>();
        }

        public object Create(Type type, params object[]? args)
        {
            var ctor = GetCctor(type, args?.Select(x => x.GetType()).ToArray());
            return ctor.DynamicInvoke(args)!;
        }

        private Delegate GetCctor(Type type, params Type[]? cctorParamTypes)
        {
            if (!_cache.TryGetValue(type, out var cctor))
            {
                cctor = MakeCreator(type, cctorParamTypes);
                _cache.Add(type, cctor);
            }

            return cctor;
        }

        private Delegate MakeCreator(Type type, params Type[]? cctorParamTypes)
        {
            var paramTypes = cctorParamTypes ?? new Type[] { };
            var cctor = type.GetConstructor(paramTypes)!;
            var parametres = paramTypes.Select(Expression.Parameter).ToList();
            var newExp = Expression.New(cctor, parametres);
            var funExp = Expression.Lambda(newExp, parametres);
            return funExp.Compile();
        }
    }
}
