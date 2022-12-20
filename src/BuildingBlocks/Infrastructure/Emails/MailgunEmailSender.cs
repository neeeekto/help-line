using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Emails;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Infrastructure.Emails
{
    public class MailgunEmailSender : IEmailSender
    {
        private readonly IMailgunApiCaller _api;
        private readonly EmailConfiguration _configuration;

        public MailgunEmailSender(IMailgunApiCaller api, EmailConfiguration configuration)
        {
            _api = api;
            _configuration = configuration;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var content = BuildContent(message);
            await _api.PostMessage(_configuration, content);
        }

        private MultipartFormDataContent BuildContent(EmailMessage email)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(email.Subject), "subject");
            foreach (var recipient in email.To)
                content.Add(new StringContent(recipient), "to");

            content.Add(new StringContent(email.From), "from");
            foreach (var (name, file) in email.Attachments)
                content.Add(new ByteArrayContent(file), "attachment", name);


            content.Add(new StringContent(email.Content), "html");
            if (email.Meta?.References?.Any() == true)
            {
                var references = string.Join("\n\t", email.Meta.References);
                if (!string.IsNullOrEmpty(references))
                    content.Add(new StringContent(references), "h:References");
            }

            if (email.Meta?.Vars?.Any() == true)
            {
                content.Add(new StringContent(JsonConvert.SerializeObject(email.Meta.Vars)), "v:my-custom-data");
            }


            return content;
        }
    }
}
