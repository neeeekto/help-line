using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class ChangeLanguageCommandTests: TicketsTestBase
    {
        protected override string NS => nameof(ChangeLanguageCommandTests);

        [Test]
        public async Task ChangeLanguageCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var lang = "ru";
            await CreateProject(new[] {"en", "ru"});
            var ticketId = await CreateTicket(testData);

            var cmd = new ChangeLanguageAction(lang);
            await ExecuteAction(ticketId, cmd, new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));

            Assert.AreEqual(lang, ticketView.Language);

            var evt = ticketView.Events.OfType<TicketLanguagesChangedEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.AreEqual(testData.Language, evt.From);
            Assert.AreEqual(lang, evt.To);
        }
    }
}
