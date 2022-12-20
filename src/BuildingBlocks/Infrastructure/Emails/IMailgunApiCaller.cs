using System.Net.Http;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Infrastructure.Emails
{
	public interface IMailgunApiCaller
	{
		Task<string> PostMessage(EmailConfiguration configuration, HttpContent formContent);
	}
}
