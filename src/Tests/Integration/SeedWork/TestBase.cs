using System.Data;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Domain;
using NUnit.Framework;
using Serilog;

namespace HelpLine.Tests.Integration.SeedWork
{
    public class TestBase
    {
        protected string ConnectionString;

        protected ILogger Logger;

        protected IEmailSender EmailSender;

        protected ExecutionContextMock ExecutionContext;



        [SetUp]
        public async Task BeforeEachTest()
        {

        }

        private static async Task ClearDatabase(IDbConnection connection)
        {

        }

        protected static void AssertBrokenRule<TRule>(AsyncTestDelegate testDelegate) where TRule : class, IBusinessRule
        {
            var message = $"Expected {typeof(TRule).Name} broken rule";
            var businessRuleValidationException = Assert.CatchAsync<BusinessRuleValidationException>(testDelegate, message);
            if (businessRuleValidationException != null)
            {
                Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
            }
        }

        protected static void AssertEventually(IProbe probe, int timeout)
        {
            new Poller(timeout).Check(probe);
        }
    }
}
