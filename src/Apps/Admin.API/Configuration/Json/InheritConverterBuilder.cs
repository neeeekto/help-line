using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonSubTypes;
using Newtonsoft.Json;

namespace HelpLine.Apps.Admin.API.Configuration.Json
{
    public static class InheritConverterBuilder
    {
        public static JsonConverter Build<T>(string typeProperty = "$type", Assembly[] assemblies = null)
        {
            var baseType = typeof(T);
            var targetAssemblies = new List<Assembly> {baseType.Assembly};
            if (assemblies != null)
            {
                targetAssemblies.AddRange(assemblies);
            }
            var builder = JsonSubtypesConverterBuilder
                .Of(baseType, typeProperty);
            var extends = targetAssemblies.SelectMany(a => a.GetTypes().Where(myType =>
                myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(baseType)));
            foreach (var extend in extends)
            {
                builder.RegisterSubtype(extend, extend.Name);
            }
            return builder.SerializeDiscriminatorProperty().Build();
        }
    }
}
