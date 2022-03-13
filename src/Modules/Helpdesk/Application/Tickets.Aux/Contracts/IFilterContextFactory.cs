using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Contracts
{
    public interface IFilterContextFactory
    {
        Task<TicketFilterCtx> Make();
    }
}
