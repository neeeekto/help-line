using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Application.Emails
{
    public interface IEmailSender
    {
        Task SendEmail(EmailMessage message);
    }
}
