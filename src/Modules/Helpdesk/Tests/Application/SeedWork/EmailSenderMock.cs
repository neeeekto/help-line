using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Emails;

namespace HelpLine.Modules.Helpdesk.Tests.Application.SeedWork
{
    internal class EmailSenderMock : IEmailSender
    {
        public Task SendEmail(EmailMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
