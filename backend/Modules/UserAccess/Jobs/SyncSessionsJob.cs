using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Modules.UserAccess.Jobs
{
    public class SyncSessionsJob : JobTask
    {
        public SyncSessionsJob(Guid id) : base(id)
        {
        }
    }
}
