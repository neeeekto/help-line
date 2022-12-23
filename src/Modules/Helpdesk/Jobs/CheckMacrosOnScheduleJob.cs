using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Modules.Helpdesk.Jobs
{
    public class CheckMacrosOnScheduleJob : JobTask
    {
        public CheckMacrosOnScheduleJob(Guid id) : base(id)
        {
        }
    }
}
