using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelpLine.BuildingBlocks.Application.Emails;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.Tests.Application.Unit.Emails
{
    [TestFixture]
    public class EmailsTests
    {
        [TestCase(null, null, null, null)]
        [TestCase("", "", "", "")]
        [TestCase(null, "null@nul.ru", "asd", null)]
        [TestCase("null@nul.com", null, "asd", null)]
        [TestCase("null@nul.com", "null@nul.com", null, null)]
        [TestCase("null@nul.com", "null@nul.com", "subject", null)]
        [TestCase("null", "null@nul.ru", "subject", "test")]
        [TestCase("null@null.com", "", "subject", "test")]
        [TestCase("null@null.com", "null", "subject", "test")]
        [TestCase("null@null.com", "null.com", "subject", "test")]
        [TestCase("null@null.com", "null@null.com", "", "test")]
        [TestCase("null@null.com", "null@null.com", "test", "")]
        public void CreateEmails_ValidationFailed_Test(string? from, string? to, string? subject, string? content)
        {
            Assert.Catch(() =>
            {
                var _ =new EmailMessage(from, new []{to}, subject, content);
            });
        }

        [Test]
        public void CreateEmails_ValidationSuccess_Test()
        {
            var array = new byte[64];

            var from = "from@test.com";
            var to = new [] {"from@test.com", "from2@test.com"};
            var subject = "Test";
            var content = "Test";
            var attachments = new ReadOnlyDictionary<string, byte[]>(new Dictionary<string, byte[]>
            {
                {"test", array}
            });
            var references = new string[] {"test"};
            var vars = new Dictionary<string, string>
            {
                {"varsTest", "vatTestValue"}
            };

            var meta = new EmailMessage.EmailMeta(references, vars);


            var email = new EmailMessage(from, to, subject, content, attachments, meta);

            Assert.AreEqual(from, email.From);
            Assert.AreEqual(to, email.To);
            Assert.AreEqual(subject, email.Subject);
            Assert.AreEqual(content, email.Content);
            Assert.AreEqual(attachments, email.Attachments);
            Assert.AreEqual(meta, email.Meta);
        }

    }
}
