using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.IntegrationTests.Tickets;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork
{
    public class TicketTestData
    {
        public string ProjectId;
        public string Language;
        public string UserId;
        public InitiatorDto Initiator;
        public string Tag;
        public string Source;
        public IEnumerable<string> Tags;
        public string Channel;
        public Dictionary<string, string> UserMeta;
        public Dictionary<string, string> Channels;
        public MessageDto Message;
        public Guid OperatorId;

        public TicketTestData()
        {
            ProjectId = TicketsTestBase.ProjectId;
            Language ="en";
            UserId = "test@test.test";
            Initiator = new UserInitiatorDto {UserId = UserId};
            Tag = "test";
            Source = "test";
            Tags = new List<string>() {Tag};
            Channel = "email";
            UserMeta = new Dictionary<string, string> {{"test", "test"}};
            Message = new MessageDto {Text = "test"};
            OperatorId = Guid.NewGuid();
            Channels = new Dictionary<string, string>() {{UserId, Channel}};
        }
    }
}
