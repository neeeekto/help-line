using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketLanguagesTests : TicketTestsBase
    {
        [Test]
        public async Task When_ChangeLanguageToUnknown_BreakWithLanguageIsExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ServiceProvider.Checker.LanguageIsExist(default, default).ReturnsForAnyArgs(Task.FromResult(false));
            AssertBrokenRule<LanguageIsExistRule>(() =>
                ticket.Execute(new ChangeLanguageTicketCommand(new LanguageCode("FR")), ServiceProvider,
                    new SystemInitiator()));
        }

        [Test]
        public async Task When_ChangeLanguageToExist_Success()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ServiceProvider.Checker.LanguageIsExist(default, default).ReturnsForAnyArgs(Task.FromResult(true));
            var language = new LanguageCode("FR");
            await ticket.Execute(new ChangeLanguageTicketCommand(language), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketLanguageChangedEvent>(ticket);
            Assert.That(evt.Language, Is.EqualTo(language));
            Assert.That(evt.Initiator, Is.TypeOf<SystemInitiator>());
        }
    }
}
