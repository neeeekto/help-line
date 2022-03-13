using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Jobs;

namespace HelpLine.Apps.Admin.API.Meta
{
    internal class CollectEmailMessagesJobDataDescription : Description
    {
        public CollectEmailMessagesJobDataDescription() : base(new DescriptionClassMap[]
        {
            new CollectEmailMessagesJobDataMap(),
            new EmailAuthSettingsMap(),
            new ReaderSettingsMap()
        }, typeof(CollectEmailMessagesJobData))
        {
        }

        class CollectEmailMessagesJobDataMap : DescriptionClassMap<CollectEmailMessagesJobData>
        {
            public override void Init()
            {
                MapField(x => x.Email);
                MapField(x => x.Project);
                MapField(x => x.AuthSettings);
                MapField(x => x.ReaderSettings);
            }
        }

        class EmailAuthSettingsMap : DescriptionClassMap<EmailAuthSettings>
        {
            public override void Init()
            {
                MapField(x => x.Token);
                MapField(x => x.ApplicationName);
                MapField(x => x.ClientId);
                MapField(x => x.ClientSecret);
            }
        }

        class ReaderSettingsMap : DescriptionClassMap<ReaderSettings>
        {
            public override void Init()
            {
                MapField(x => x.Filter);
                MapField(x => x.Locale);
                MapField(x => x.Tags);
                MapField(x => x.FailedLabels);
                MapField(x => x.SuccessLabels);
            }
        }
    }
}
