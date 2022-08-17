using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Jobs;
using HelpLine.Modules.UserAccess.Jobs;

namespace HelpLine.Apps.Admin.API.Meta.Jobs
{
    public class JobTasksDescRegistry : ReadOnlyDictionary<string, Description>
    {

        public JobTasksDescRegistry() : base(new Dictionary<string, Description>()
        {
            {typeof(RunTicketTimersJob).FullName, null},
            {typeof(RemoveAutobanJob).FullName, null},
            {typeof(CheckMacrosOnScheduleJob).FullName, null},
            {typeof(CollectEmailMessagesJob).FullName, new CollectEmailMessagesJobDataDescription()},
            {typeof(ClearZombieSessionsJob).FullName, null},
            {typeof(SyncSessionsJob).FullName, null},
        })
        {
        }
    }
}
