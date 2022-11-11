using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Modules.Helpdesk.Jobs
{
    public class RunTicketTimersJob : JobTask
    {
        public RunTicketTimersJob(Guid id) : base(id)
        {
        }
    }
}
