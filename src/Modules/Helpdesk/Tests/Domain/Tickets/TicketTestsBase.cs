using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Domain.SeedWork;
using NSubstitute;

namespace HelpLine.Modules.Helpdesk.Tests.Domain.Tickets
{
    public abstract class TicketTestsBase : EventSourcingTestBase<TicketId>
    {
        protected static TicketId TicketId = new("0-000000");

        protected TicketServiceProvider ServiceProvider = new TicketServiceProvider(TicketId);

        protected void ClearServices()
        {
            ServiceProvider = new TicketServiceProvider(TicketId);
        }

        protected Task<Ticket> MakeTicket(TestData data)
        {
            return Ticket.Create(ServiceProvider.IdFactory, ServiceProvider,
                data.ProjectId,
                data.LanguageCode,
                data.Initiator,
                data.Tags,
                data.UserChannels,
                data.UserMeta,
                data.Meta,
                data.Message
            );
        }

        protected class TestData
        {
            public ProjectId ProjectId;
            public LanguageCode LanguageCode;
            public UserId UserId;
            public Initiator Initiator;
            public Tag Tag;
            public IEnumerable<Tag> Tags;
            public Channel Channel;
            public UserChannels UserChannels;
            public UserMeta UserMeta;
            public Message Message;
            public OperatorId OperatorId;
            public TicketMeta Meta;

            public TestData()
            {
                ProjectId = new ProjectId("test");
                LanguageCode = new LanguageCode("ru");
                UserId = new UserId("test@test.test");
                Initiator = new UserInitiator(UserId);
                Tag = new Tag("test");
                Tags = new List<Tag>() {Tag};
                Channel = new Channel("email");
                UserChannels = new UserChannels(new[] {new UserChannel(UserId, Channel),});
                UserMeta = new UserMeta(new Dictionary<string, string> {{"test", "test"}});
                Message = new Message("test");
                OperatorId = new OperatorId(Guid.NewGuid());
                Meta = new TicketMeta("web", null, null);
            }
        }
    }
}
