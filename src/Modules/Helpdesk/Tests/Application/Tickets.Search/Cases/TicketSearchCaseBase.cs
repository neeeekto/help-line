using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Search.Cases;

public abstract class TicketSearchCaseBase
{
    public virtual string? Descritpion { get; }
    public virtual string? Name { get; }
    protected List<string> ExpectTickets = new ();

    public abstract Task<TicketFilterBase> Prepare(TicketsTestBase ctx);

    public virtual Task<IReadOnlyList<string>> Expect(TicketsTestBase ctx)
    {
        return Task.FromResult<IReadOnlyList<string>>(ExpectTickets);
    }
}
