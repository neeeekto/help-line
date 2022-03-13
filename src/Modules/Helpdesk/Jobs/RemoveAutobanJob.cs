using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Modules.Helpdesk.Jobs
{
    public class RemoveAutobanJob : JobTask
    {
        public RemoveAutobanJob(Guid id) : base(id)
        {
        }
    }
}
