using System;

namespace HelpLine.BuildingBlocks.Application.Emails
{
    public class SendEmailMessageException : Exception
    {
        public SendEmailMessageException(string? message) : base(message)
        {
        }
    }
}
