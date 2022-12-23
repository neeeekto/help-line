using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Jobs;
using HelpLine.Modules.Quality.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Jobs;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.Migrations.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace HelpLine.Apps.Admin.API.Configuration.Json
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
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<ChannelSettings>());
                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<MigrationStatus>());

                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<JobDataBase>(assemblies: new[]
                {
                    typeof(RunTicketTimersJob).Assembly,
                    typeof(ClearZombieSessionsJob).Assembly,
                }));

                opt.SerializerSettings.Converters.Add(InheritConverterBuilder.Build<MigrationParams>(assemblies: new[]
                {
                    typeof(IHelpdeskModule).Assembly,
                    typeof(IUserAccessModule).Assembly,
                    typeof(IQualityModule).Assembly,
                    typeof(JsonExtensions).Assembly,
                }));
                opt.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
        }
    }
}
