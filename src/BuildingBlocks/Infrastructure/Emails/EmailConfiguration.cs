namespace HelpLine.BuildingBlocks.Infrastructure.Emails
{
    public class EmailConfiguration
    {
        public string Key { get; }
        public string Domain { get; }

        public EmailConfiguration(string key, string domain)
        {
            Key = key;
            Domain = domain;
        }
    }
}
