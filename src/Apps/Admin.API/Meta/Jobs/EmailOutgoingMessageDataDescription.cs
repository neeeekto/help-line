using System;
using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;

namespace HelpLine.Apps.Admin.API.Meta
{
    internal class EmailOutgoingMessageDataDescription : Description
    {
        public EmailOutgoingMessageDataDescription() : base(new DescriptionClassMap[]
        {
            new EmailOutgoingMessageDataMap(),
            new DiscussMessageMap(),
        }, typeof(EmailOutgoingMessageData))
        {
        }

        private class EmailOutgoingMessageDataMap : DescriptionClassMap<EmailOutgoingMessageData>
        {
            public override void Init()
            {
                MapField(x => x.ProjectId);
                MapField(x => x.TicketId);
                MapField(x => x.Language);
                MapField(x => x.Messages);
            }
        }

        private class DiscussMessageMap : DescriptionClassMap<EmailOutgoingMessageData.DiscussMessage>
        {
            public override void Init()
            {
                MapField(x => x.Text);
                MapField(x => x.FromUser);
                MapField(x => x.Attachments);
            }
        }
    }
}
