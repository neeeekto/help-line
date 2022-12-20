using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Emails;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Infrastructure.UnitTests.Emails
{
    [TestFixture]
    public class MailgunEmailSenderTests
    {
        private IMailgunApiCaller apiCaller = Substitute.For<IMailgunApiCaller>();

        [Test]
        public async Task SendEmail_CheckBody_Success()
        {
            var attachments = new ReadOnlyDictionary<string, byte[]>(new Dictionary<string, byte[]>
            {
                {"test", new byte[0]}
            });
            var references = new string[] {"test"};
            var vars = new Dictionary<string, string>
            {
                {"varsTest", "vatTestValue"}
            };
            var meta = new EmailMessage.EmailMeta(references, vars);


            var email = new EmailMessage("from@test.test", new []{"to@test.com"}, "test", "test", attachments, meta);
            var config = new EmailConfiguration("qwerty", "test");
            apiCaller.PostMessage(default, default).ReturnsForAnyArgs(Task.FromResult("Success"));
            var emailSender = new MailgunEmailSender(apiCaller, config);
            await emailSender.SendEmail(email);
            HttpContent content = default;
            await apiCaller.Received(1).PostMessage(Arg.Is(config), Arg.Any<HttpContent>());
        }
    }
}
