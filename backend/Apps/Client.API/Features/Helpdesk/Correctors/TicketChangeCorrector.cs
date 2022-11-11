using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Correctors;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Correctors
{
    //TODO: Make me. I need remove private note if user doesn't have access
    public class TicketChangeCorrector : IDataCorrector<TicketView>
    {
        public Task Correct(TicketView data)
        {
            return Task.CompletedTask;
        }
    }
}
