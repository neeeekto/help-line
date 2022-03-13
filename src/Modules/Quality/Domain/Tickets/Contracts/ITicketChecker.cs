using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Contracts
{
    public interface ITicketChecker
    {
        Task<bool> CheckByIndicators(IEnumerable<Indicator> indicators);
    }
}
