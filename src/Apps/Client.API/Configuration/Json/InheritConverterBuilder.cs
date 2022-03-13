using System.Linq;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using JsonSubTypes;
using Newtonsoft.Json;

namespace HelpLine.Apps.Client.API.Configuration.Json
{
    public static class InheritConverterBuilder
    {
        public static JsonConverter Build<T>(string typeProperty = "$type")
        {
            var baseType = typeof(T);
            var builder = JsonSubtypesConverterBuilder
                .Of(baseType, typeProperty);
            var extends = baseType.Assembly.GetTypes().Where(myType =>
                myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(baseType));
            foreach (var extend in extends)
            {
                builder.RegisterSubtype(extend, extend.Name);
            }
            return builder.SerializeDiscriminatorProperty().Build();
        }
    }
}
