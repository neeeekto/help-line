using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReminders
{
    public class GetTicketRemindersQuery : QueryBase<IEnumerable<TicketReminderEntity>>
    {
        public string ProjectId { get; }
        public bool? Enabled { get; }

        public GetTicketRemindersQuery(string projectId, bool? enabled = null)
        {
            ProjectId = projectId;
            Enabled = enabled;
        }


    }
}
