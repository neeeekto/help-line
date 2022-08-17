namespace HelpLine.BuildingBlocks.Application.Emails
{
    public static class EmailMessageFromBuilder
    {
        public static string Build(string domain, string from, string name) => $"{name}<{from}@{domain}>";


    }
}
