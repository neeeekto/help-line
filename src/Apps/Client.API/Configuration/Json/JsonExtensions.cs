using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace HelpLine.Apps.Client.API.Configuration.Json
{
    public static class JsonExtensions
    {

        public static void AddJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson((opt) =>
            {
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<TicketEventView>());
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<InitiatorView>());
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<TicketActionBase>());
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<TicketReminderDto>());
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<DescriptionFieldType>());
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<ScheduledEventResultView>());
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<TicketReminderItemBase>());
                opt.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }
    }
}
