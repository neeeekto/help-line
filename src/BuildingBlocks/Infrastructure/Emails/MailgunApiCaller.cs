using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Emails;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Infrastructure.Emails
{
	public class MailgunApiCaller : IMailgunApiCaller
	{
		protected const string BaseAddress = "api.mailgun.net/v3";



		public async Task<string> PostMessage(EmailConfiguration configuration, HttpContent formContent)
		{
			using var client = new HttpClient();
			var buildUri = new UriBuilder
			{
				Host = BaseAddress,
				Scheme = "https",
				Path = $"{configuration.Domain}/messages"
			};

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
				Convert.ToBase64String(Encoding.UTF8.GetBytes("api:" + configuration.Key)));

			var response = await client.PostAsync(buildUri.ToString(), formContent);
			var responseContent = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				throw HandleBadResponse(responseContent);
			}

			dynamic responseObj = JsonConvert.DeserializeObject(responseContent);
			string messageId = responseObj.id;
			return messageId;
		}

		private static Exception HandleBadResponse(string response)
		{
			try
			{
				dynamic content = JsonConvert.DeserializeObject(response);
				var msg = content.message != null
					? content.message.ToString() as string
					: "Failed to send email via mailgun";
				return new SendEmailMessageException(msg);
			}
			catch (Exception)
			{
				return new SendEmailMessageException($"Failed to send email via mailgun: {response}");
			}
		}
	}
}
