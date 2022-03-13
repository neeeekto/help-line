using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Modules.UserAccess.Jobs
{
    public class ClearZombieSessionsJob : JobTask
    {
        public ClearZombieSessionsJob(Guid id) : base(id)
        {
        }
    }
}
