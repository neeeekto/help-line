using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketMessageTemplate;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketMessageTemplate;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketMessageTemplates;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [TestFixture]
    public class RemoveTicketMessageTemplateCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(RemoveFromUnsubscribedCommandTests);


        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private Task<Guid> CreateTemplate()
        {
            return Module.ExecuteCommandAsync(new CreateTicketMessageTemplateCommand(ProjectId,
                new Dictionary<string, TicketMessageTemplateContent?>()
                {
                    {
                        EngLangKey, new TicketMessageTemplateContent()
                        {
                            Message = new MessageDto {Text = "test"},
                            Title = "test"
                        }
                    }
                }));
        }

        [Test]
        public async Task RemoveFromUnsubscribedCommand_WhenIsValid_IsSuccessful()
        {
            var templateId = await CreateTemplate();
            await Module.ExecuteCommandAsync(new RemoveTicketMessageTemplateCommand(templateId));

            var entities = await Module.ExecuteQueryAsync(new GetTicketMessageTemplatesQuery(ProjectId));
            Assert.That(entities, Is.Empty);
        }
    }
}
