using System;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Modules.Helpdesk.Jobs
{
    public class EmailAuthSettings
    {
        public string Token { get; set; }
        public string ApplicationName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class ReaderSettings
    {
        public string Locale { get; set; }
        public string Filter { get; set; }
        public string SuccessLabels { get; set; }
        public string FailedLabels { get; set; }
        public string Tags { get; set; }
    }
    public class CollectEmailMessagesJobData : JobDataBase
    {
        public string Email { get; set; }
        public string Project { get; set; }
        public ReaderSettings ReaderSettings { get; set; }
        public EmailAuthSettings AuthSettings { get; set; }
    }

    public class CollectEmailMessagesJob : JobTask<CollectEmailMessagesJobData>
    {
        public CollectEmailMessagesJob(Guid id, CollectEmailMessagesJobData data) : base(id, data)
        {
        }
    }
}
