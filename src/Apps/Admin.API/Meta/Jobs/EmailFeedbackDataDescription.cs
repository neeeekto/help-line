using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;

namespace HelpLine.Apps.Admin.API.Meta.Jobs
{
    internal class EmailFeedbackDataDescription : Description
    {
        public EmailFeedbackDataDescription() : base(new DescriptionClassMap[]
        {
            new EmailFeedbackDataMap(),
        }, typeof(EmailFeedbackData))
        {
        }

        class EmailFeedbackDataMap : DescriptionClassMap<EmailFeedbackData>
        {
            public override void Init()
            {
                MapField(x => x.FeedbackId);
                MapField(x => x.Language);
                MapField(x => x.ProjectId);
                MapField(x => x.TicketId);
            }
        }
    }
}
